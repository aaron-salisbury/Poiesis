using PoiesisDB.Core.Base;
using PoiesisDB.Core.Data;
using System;
using System.Collections.Generic;
using System.IO;

namespace PoiesisDB.Core.Models
{
    public class DatabaseAttributes : ObservableObject
    {
        public string[] TransferTypes = { "Schema and Data", "Schema Only" };
        public bool SchemaOnly { get; set; }
        public string DataSource { get; set; }
        public string SourceConnectionString { get; set; }
        public string NewConnectionString { get; set; }
        public Dictionary<SQLServerSystemFile.Types, SQLServerSystemFile> SQLServerSystemFilesByTypes { get; set; }

        private string _newDatabaseName;
        public string NewDatabaseName
        {
            get { return _newDatabaseName; }
            set
            {
                _newDatabaseName = value;
                NewConnectionString = $"Data Source={DataSource};Initial Catalog={NewDatabaseName};Integrated Security=True;";

                if (SQLServerSystemFilesByTypes != null)
                {
                    SQLServerSystemFilesByTypes[SQLServerSystemFile.Types.Data].Name = NewDatabaseName;
                    SQLServerSystemFilesByTypes[SQLServerSystemFile.Types.Data].File = $"{NewDatabaseName}{Path.GetExtension(SQLServerSystemFilesByTypes[SQLServerSystemFile.Types.Data].File)}";
                    SQLServerSystemFilesByTypes[SQLServerSystemFile.Types.Log].Name = $"{NewDatabaseName}_log";
                    SQLServerSystemFilesByTypes[SQLServerSystemFile.Types.Log].File = $"{NewDatabaseName}_log{Path.GetExtension(SQLServerSystemFilesByTypes[SQLServerSystemFile.Types.Log].File)}";
                }

                RaisePropertyChanged("NewDatabaseName");
            }
        }

        public DatabaseAttributes(Manager manager)
        {
            DataSource = manager.SelectDatabase.DataSource;
            SourceConnectionString = $"Data Source={DataSource};Initial Catalog={manager.SelectDatabase.SourceDatabaseName};Integrated Security=True;";

            SQLServerSystemFilesByTypes = DataAccess.GetSQLServerSystemFilesByTypes(SourceConnectionString, manager.Logger);

            if (SQLServerSystemFilesByTypes != null)
            {
                NewDatabaseName = $"Poiesis_{DateTime.Now:yyyyMMddHHmmss}";
            }
        }
    }
}
