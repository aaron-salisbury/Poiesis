using Poiesis.Core.Base;
using Poiesis.Core.Base.Extensions;
using Poiesis.Core.Domains;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace Poiesis.Core
{
    public class Manager : ObservableObject
    {
        private readonly ILogger _logger;

        public DatabaseFacade SourceDatabase { get; set; }
        public DatabaseFacade NewDatabase { get; set; }

        private ObservableCollection<string> _localDatabaseNames;
        public ObservableCollection<string> LocalDatabaseNames
        {
            get
            {
                return _localDatabaseNames;
            }
            set
            {
                _localDatabaseNames = value;
                RaisePropertyChanged();
            }
        }

        public enum TransferTypes
        {
            [Display(Name = "Schema and Data")]
            SchemaAndData,
            [Display(Name = "Schema Only")]
            SchemaOnly
        }

        private TransferTypes _transferType;
        public TransferTypes TransferType
        {
            get { return _transferType; }
            set
            {
                _transferType = value;
                RaisePropertyChanged();
            }
        }

        public static string GetTransferTypeDisplayName(TransferTypes transferType)
        {
            return transferType.GetAttribute<DisplayAttribute>()?.Name ?? transferType.ToString();
        }

        public Manager(AppLogger appLogger)
        {
            _logger = appLogger.Logger;

            SourceDatabase = new DatabaseFacade
            {
                DataSource = "localhost"
            };

            TransferType = TransferTypes.SchemaAndData;
        }

        public bool SetLocalDatabaseNames()
        {
            try
            {
                _logger.Information("Gathering names of local databases.");

                LocalDatabaseNames = new ObservableCollection<string>(DataAccess.GetLocalDatabaseNames(SourceDatabase.DataSource, _logger).OrderBy(ldn => ldn));

                return LocalDatabaseNames != null && LocalDatabaseNames.Count > 0;
            }
            catch (Exception e)
            {
                _logger.Error($"Failed to gather names of local databases: {e.Message}");
                return false;
            }
        }

        public bool SetSourceDatabaseSQLServerSystemFiles()
        {
            try
            {
                _logger.Information("Gathering SQL Server System Files from source database.");

                SourceDatabase.SQLServerSystemFilesByTypes = DataAccess.GetSQLServerSystemFilesByTypes(SourceDatabase.ConnectionString, _logger);

                return true;
            }
            catch (Exception e)
            {
                _logger.Error($"Failed to gather SQL Server System Files from source database: {e.Message}");
                return false;
            }
        }

        public bool SetNewDatabase()
        {
            try
            {
                _logger.Information("Beginning to setup new database configurations.");

                NewDatabase = new DatabaseFacade
                {
                    Name = $"Poiesis_{DateTime.Now:yyyyMMddHHmmss}",
                    DataSource = SourceDatabase.DataSource
                };

                return true;
            }
            catch (Exception e)
            {
                _logger.Error($"Failed to setup new database configurations: {e.Message}");
                return false;
            }
        }

        public void UpdateNewDatabaseSQLServerSystemFiles(string newDatabaseName)
        {
            NewDatabase.Name = newDatabaseName;

            Dictionary<SQLServerSystemFile.Types, SQLServerSystemFile> sqlServerSystemFilesByTypes = new Dictionary<SQLServerSystemFile.Types, SQLServerSystemFile>(SourceDatabase.SQLServerSystemFilesByTypes);
            
            if (sqlServerSystemFilesByTypes != null && sqlServerSystemFilesByTypes.ContainsKey(SQLServerSystemFile.Types.Data) && sqlServerSystemFilesByTypes.ContainsKey(SQLServerSystemFile.Types.Log))
            {
                sqlServerSystemFilesByTypes[SQLServerSystemFile.Types.Data].Name = NewDatabase.Name;
                sqlServerSystemFilesByTypes[SQLServerSystemFile.Types.Data].File = $"{NewDatabase.Name}{Path.GetExtension(sqlServerSystemFilesByTypes[SQLServerSystemFile.Types.Data].File)}";
                sqlServerSystemFilesByTypes[SQLServerSystemFile.Types.Log].Name = $"{NewDatabase.Name}_log";
                sqlServerSystemFilesByTypes[SQLServerSystemFile.Types.Log].File = $"{NewDatabase.Name}_log{Path.GetExtension(sqlServerSystemFilesByTypes[SQLServerSystemFile.Types.Log].File)}";

                NewDatabase.SQLServerSystemFilesByTypes = sqlServerSystemFilesByTypes;
            }
        }

        public bool InitiatePoiesis()
        {
            try
            {
                _logger.Information("Initiating database clone.");

                bool dbCreated = DataAccess.CreateDatabase(NewDatabase, _logger);

                bool schemaTransfered = true;
                if (dbCreated)
                {
                    schemaTransfered = DataAccess.TransferSchema(SourceDatabase.ConnectionString, NewDatabase.ConnectionString, _logger);
                }

                bool dataTransfered = true;
                if (dbCreated && schemaTransfered && TransferType == TransferTypes.SchemaAndData)
                {
                    dataTransfered = DataAccess.TransferData(SourceDatabase.ConnectionString, NewDatabase.ConnectionString, _logger);
                }

                return dbCreated && schemaTransfered && dataTransfered;
            }
            catch (Exception e)
            {
                _logger.Error($"Failed to clone database: {e.Message}");
                return false;
            }
        }
    }
}
