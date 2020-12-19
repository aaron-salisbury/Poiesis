using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using PoiesisDB.Core.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace PoiesisDB.Core.Data
{
    public class DataAccess
    {
        public static List<string> GetLocalDatabaseNames(string dataSource, ILogger logger)
        {
            const short GREATEST_SYSTEM_DB_ID = 4;

            List<string> localDatabaseNames = new List<string>();

            try
            {
                using (SqlConnection con = new SqlConnection($"Data Source={dataSource}; Integrated Security=True;"))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand($"SELECT name FROM sys.databases WHERE database_id > {GREATEST_SYSTEM_DB_ID}", con))
                    {
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                string databaseName = dr[0].ToString();

                                if (!string.IsNullOrEmpty(databaseName))
                                {
                                    localDatabaseNames.Add(dr[0].ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                return null;
            }

            return localDatabaseNames;
        }

        public static Dictionary<SQLServerSystemFile.Types, SQLServerSystemFile> GetSQLServerSystemFilesByTypes(string sourceConnectionString, ILogger logger)
        {
            Dictionary<SQLServerSystemFile.Types, SQLServerSystemFile> sqlServerSystemFilesByTpes = new Dictionary<SQLServerSystemFile.Types, SQLServerSystemFile>();

            try
            {
                using (SqlConnection con = new SqlConnection(sourceConnectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand($"SELECT * FROM sys.database_files", con))
                    {
                        using (SqlDataReader sqlDataReader = cmd.ExecuteReader())
                        {
                            bool readFromDataRow = false;

                            while (sqlDataReader.Read())
                            {
                                SQLServerSystemFile.Types type;

                                if (!readFromDataRow)
                                {
                                    type = SQLServerSystemFile.Types.Data;
                                    readFromDataRow = true;
                                }
                                else
                                {
                                    type = SQLServerSystemFile.Types.Log;
                                }

                                sqlServerSystemFilesByTpes[type] = SQLServerSystemFile.CreateFromSqlDataReader(sqlDataReader, type, logger);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                return null;
            }

            return sqlServerSystemFilesByTpes;
        }

        public static bool CreateDatabase(DatabaseAttributes databaseAttributes, ILogger logger)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection($"Server={databaseAttributes.DataSource};Integrated security=SSPI;database=master"))
                {
                    connection.Open();

                    string cmdText = $"CREATE DATABASE {databaseAttributes.NewDatabaseName} ON PRIMARY " +
                        $"(NAME = {databaseAttributes.NewDatabaseName}, " +
                        $"FILENAME = '{Path.Combine(databaseAttributes.SQLServerSystemFilesByTypes[SQLServerSystemFile.Types.Data].Path, databaseAttributes.SQLServerSystemFilesByTypes[SQLServerSystemFile.Types.Data].File)}', " +
                        $"SIZE = {databaseAttributes.SQLServerSystemFilesByTypes[SQLServerSystemFile.Types.Data].Size}, " +
                        $"MAXSIZE = {databaseAttributes.SQLServerSystemFilesByTypes[SQLServerSystemFile.Types.Data].MaxSize}, " +
                        $"FILEGROWTH = {databaseAttributes.SQLServerSystemFilesByTypes[SQLServerSystemFile.Types.Data].Growth}) " +
                        $"LOG ON (NAME = {Path.GetFileNameWithoutExtension(databaseAttributes.SQLServerSystemFilesByTypes[SQLServerSystemFile.Types.Log].File)}, " +
                        $"FILENAME = '{Path.Combine(databaseAttributes.SQLServerSystemFilesByTypes[SQLServerSystemFile.Types.Log].Path, databaseAttributes.SQLServerSystemFilesByTypes[SQLServerSystemFile.Types.Log].File)}', " +
                        $"SIZE = {databaseAttributes.SQLServerSystemFilesByTypes[SQLServerSystemFile.Types.Log].Size}, " +
                        $"MAXSIZE = {databaseAttributes.SQLServerSystemFilesByTypes[SQLServerSystemFile.Types.Log].MaxSize}, " +
                        $"FILEGROWTH = {databaseAttributes.SQLServerSystemFilesByTypes[SQLServerSystemFile.Types.Log].Growth})";

                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                return false;
            }

            logger.Information("Created New Database");
            return true;
        }

        public static bool TransferSchema(DatabaseAttributes databaseAttributes, ILogger logger)
        {
            try
            {
                ServerConnection sourceConnection = new ServerConnection(new Microsoft.Data.SqlClient.SqlConnection(databaseAttributes.SourceConnectionString));
                Server sourceServer = new Server(sourceConnection);

                ServerConnection destinationConnection = new ServerConnection(new Microsoft.Data.SqlClient.SqlConnection(databaseAttributes.NewConnectionString));
                Server destinationServer = new Server(destinationConnection);

                InitiateServer(sourceServer);
                InitiateServer(destinationServer);

                Database sourceDatabase = sourceServer.Databases[sourceServer.ConnectionContext.DatabaseName];
                Database destinationDatabase = destinationServer.Databases[destinationServer.ConnectionContext.DatabaseName];

                foreach (Schema schema in sourceDatabase.Schemas)
                {
                    if (schema.IsSystemObject) { continue; }

                    Microsoft.Data.SqlClient.SqlCommand sqlCommand = new Microsoft.Data.SqlClient.SqlCommand($"CREATE SCHEMA [{schema.Name}]", destinationConnection.SqlConnectionObject);
                    sqlCommand.ExecuteNonQuery();
                }

                Transfer transfer = new Transfer(sourceDatabase)
                {
                    DestinationServer = destinationConnection.ServerInstance,
                    DestinationDatabase = destinationConnection.DatabaseName,
                    DestinationLogin = destinationConnection.Login,
                    DestinationPassword = destinationConnection.Password,
                    CopySchema = true,
                    CopyData = false,
                    DropDestinationObjectsFirst = false
                };

                transfer.Options.ContinueScriptingOnError = true;
                transfer.Options.NoFileGroup = true;
                transfer.Options.NoExecuteAs = true;
                transfer.Options.WithDependencies = false;
                transfer.Options.DriDefaults = true;

                HashSet<string> schemaNames = new HashSet<string>();

                foreach (TransferSchemaObject schemaObject in GetSqlObjects(sourceDatabase))
                {
                    ResetTransfer(transfer);
                    transfer.ObjectList.Clear();
                    transfer.ObjectList.Add(schemaObject.NamedSmoObject);

                    foreach (string script in transfer.ScriptTransfer())
                    {
                        Microsoft.Data.SqlClient.SqlCommand sqlCommand = new Microsoft.Data.SqlClient.SqlCommand(script, destinationConnection.SqlConnectionObject);
                        sqlCommand.ExecuteNonQuery();
                    }

                    if (string.Equals(schemaObject.Type, "Table"))
                    {
                        ApplyIndexesForeignKeysChecks(destinationDatabase, schemaObject.NamedSmoObject);
                    }

                    schemaObject.Complete = true;
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                return false;
            }

            logger.Information("Transfered Schema from Source Database");
            return true;
        }

        public static bool TransferData(DatabaseAttributes databaseAttributes, ILogger logger)
        {
            try
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(databaseAttributes.NewConnectionString) { BatchSize = 500, NotifyAfter = 1000 })
                {
                    ServerConnection sourceConnection = new ServerConnection(new Microsoft.Data.SqlClient.SqlConnection(databaseAttributes.SourceConnectionString));
                    Server sourceServer = new Server(sourceConnection);

                    InitiateServer(sourceServer);

                    foreach (Table item in sourceServer.Databases[sourceServer.ConnectionContext.DatabaseName].Tables)
                    {
                        if (!item.IsSystemObject)
                        {
                            TransferDataObject transferDataObject = new TransferDataObject
                            {
                                Table = item.Name,
                                SqlCommand = $"SELECT * FROM {item.Name} WITH(NOLOCK)"
                            };

                            using (Microsoft.Data.SqlClient.SqlCommand sqlCommand = new Microsoft.Data.SqlClient.SqlCommand(transferDataObject.SqlCommand, sourceConnection.SqlConnectionObject))
                            using (Microsoft.Data.SqlClient.SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                            {
                                bulkCopy.DestinationTableName = transferDataObject.Table;
                                bulkCopy.WriteToServer(sqlDataReader);

                                transferDataObject.Complete = true;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                return false;
            }

            logger.Information("Transfered Data from Source Database");
            return true;
        }

        private static void InitiateServer(Server server)
        {
            // Smo is really slow if we don't set the default properties upon partial instantiation.
            server.SetDefaultInitFields(typeof(Table), "IsSystemObject", "Name");
            server.SetDefaultInitFields(typeof(StoredProcedure), "IsSystemObject", "Name");
            server.SetDefaultInitFields(typeof(UserDefinedFunction), "IsSystemObject", "Name");
            server.SetDefaultInitFields(typeof(Microsoft.SqlServer.Management.Smo.View), "IsSystemObject", "Name");
            server.SetDefaultInitFields(typeof(Column), "Identity");
            server.SetDefaultInitFields(typeof(Index), "IndexKeyType");
        }

        private static List<TransferSchemaObject> GetSqlObjects(Database database)
        {
            List<TransferSchemaObject> schemaObjects = new List<TransferSchemaObject>();

            foreach (SqlAssembly sqlAssembly in database.Assemblies)
            {
                if (!sqlAssembly.IsSystemObject)
                {
                    schemaObjects.Add(new TransferSchemaObject { Name = sqlAssembly.Name, NamedSmoObject = sqlAssembly, Type = sqlAssembly.GetType().Name });
                }
            }

            foreach (UserDefinedDataType dataType in database.UserDefinedDataTypes)
            {
                schemaObjects.Add(new TransferSchemaObject { Name = dataType.Name, NamedSmoObject = dataType, Type = dataType.GetType().Name });
            }

            foreach (UserDefinedTableType tableType in database.UserDefinedTableTypes)
            {
                schemaObjects.Add(new TransferSchemaObject { Name = tableType.Name, NamedSmoObject = tableType, Type = tableType.GetType().Name });
            }

            foreach (Table tables in database.Tables)
            {
                if (!tables.IsSystemObject)
                {
                    schemaObjects.Add(new TransferSchemaObject { Name = tables.Name, NamedSmoObject = tables, Type = tables.GetType().Name });
                }
            }

            foreach (Microsoft.SqlServer.Management.Smo.View smoView in database.Views)
            {
                if (!smoView.IsSystemObject)
                {
                    schemaObjects.Add(new TransferSchemaObject { Name = smoView.Name, NamedSmoObject = smoView, Type = smoView.GetType().Name });
                }
            }

            foreach (UserDefinedFunction function in database.UserDefinedFunctions)
            {
                if (!function.IsSystemObject)
                {
                    schemaObjects.Add(new TransferSchemaObject { Name = function.Name, NamedSmoObject = function, Type = function.GetType().Name });
                }
            }

            foreach (StoredProcedure storedProcedure in database.StoredProcedures)
            {
                if (!storedProcedure.IsSystemObject)
                {
                    schemaObjects.Add(new TransferSchemaObject { Name = storedProcedure.Name, NamedSmoObject = storedProcedure, Type = storedProcedure.GetType().Name });
                }
            }

            foreach (DatabaseDdlTrigger databaseDdlTrigger in database.Triggers)
            {
                if (!databaseDdlTrigger.IsSystemObject)
                {
                    schemaObjects.Add(new TransferSchemaObject { Name = databaseDdlTrigger.Name, NamedSmoObject = databaseDdlTrigger, Type = databaseDdlTrigger.GetType().Name });
                }
            }

            return schemaObjects;
        }

        private static void ResetTransfer(Transfer transfer)
        {
            transfer.CopyAllDatabaseTriggers = false;
            transfer.CopyAllDefaults = false;
            transfer.CopyAllLogins = false;
            transfer.CopyAllObjects = false;
            transfer.CopyAllPartitionFunctions = false;
            transfer.CopyAllPartitionSchemes = false;
            transfer.CopyAllRoles = false;
            transfer.CopyAllRules = false;
            transfer.CopyAllSchemas = false;
            transfer.CopyAllSqlAssemblies = false;
            transfer.CopyAllStoredProcedures = false;
            transfer.CopyAllSynonyms = false;
            transfer.CopyAllTables = false;
            transfer.CopyAllUserDefinedAggregates = false;
            transfer.CopyAllUserDefinedDataTypes = false;
            transfer.CopyAllUserDefinedFunctions = false;
            transfer.CopyAllUserDefinedTypes = false;
            transfer.CopyAllUsers = false;
            transfer.CopyAllViews = false;
            transfer.CopyAllXmlSchemaCollections = false;
            transfer.CreateTargetDatabase = false;
            //transfer.DropDestinationObjectsFirst = false;
            transfer.PrefetchObjects = false;
            transfer.SourceTranslateChar = false;
        }

        private static void ApplyIndexesForeignKeysChecks(Database destinationDatabase, NamedSmoObject namedSmoObject)
        {
            Table destinationTable = destinationDatabase.Tables[namedSmoObject.Name];

            #region Indexes
            foreach (Index sourceIndex in (namedSmoObject as Table).Indexes)
            {
                string name = sourceIndex.Name;
                Index index = new Index(destinationTable, name);
                index.IndexKeyType = sourceIndex.IndexKeyType;
                index.IsClustered = sourceIndex.IsClustered;
                index.IsUnique = sourceIndex.IsUnique;
                index.CompactLargeObjects = sourceIndex.CompactLargeObjects;
                index.IgnoreDuplicateKeys = sourceIndex.IgnoreDuplicateKeys;
                index.IsFullTextKey = sourceIndex.IsFullTextKey;
                index.PadIndex = sourceIndex.PadIndex;
                index.FileGroup = sourceIndex.FileGroup;

                foreach (IndexedColumn sourceIndexedColumn in sourceIndex.IndexedColumns)
                {
                    IndexedColumn column = new IndexedColumn(index, sourceIndexedColumn.Name, sourceIndexedColumn.Descending);
                    column.IsIncluded = sourceIndexedColumn.IsIncluded;
                    index.IndexedColumns.Add(column);
                }

                index.FileGroup = destinationTable.FileGroup ?? index.FileGroup;
                index.Create();
            }
            #endregion

            #region ForeignKeys
            foreach (ForeignKey sourceFK in (namedSmoObject as Table).ForeignKeys)
            {
                string name = sourceFK.Name;
                ForeignKey foreignkey = new ForeignKey(destinationTable, name);
                foreignkey.DeleteAction = sourceFK.DeleteAction;
                foreignkey.IsChecked = sourceFK.IsChecked;
                foreignkey.IsEnabled = sourceFK.IsEnabled;
                foreignkey.ReferencedTable = sourceFK.ReferencedTable;
                foreignkey.ReferencedTableSchema = sourceFK.ReferencedTableSchema;
                foreignkey.UpdateAction = sourceFK.UpdateAction;

                foreach (ForeignKeyColumn sourceFKColumn in sourceFK.Columns)
                {
                    string referencedColumn = sourceFKColumn.ReferencedColumn;
                    ForeignKeyColumn column = new ForeignKeyColumn(foreignkey, sourceFKColumn.Name, referencedColumn);
                    foreignkey.Columns.Add(column);
                }

                foreignkey.Create();
            }
            #endregion

            #region Checks
            foreach (Check chkConstr in (namedSmoObject as Table).Checks)
            {
                Check check = new Check(destinationTable, chkConstr.Name);
                check.IsChecked = chkConstr.IsChecked;
                check.IsEnabled = chkConstr.IsEnabled;
                check.Text = chkConstr.Text;
                check.Create();
            }
            #endregion
        }
    }
}