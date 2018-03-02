using GalaSoft.MvvmLight;
using Poiesis.Base;
using Serilog.Core;
using System.Collections.Generic;

namespace Poiesis.Model
{
    public class SelectDatabase : ObservableObject
    {
        public string DataSource { get; set; }
        public string SourceDatabaseName { get; set; }

        public SelectDatabase()
        {
            DataSource = "localhost";
        }

        public List<string> GetLocalDatabaseNames(Logger logger)
        {
            return DataAccess.GetLocalDatabaseNames(DataSource, logger);
        }
    }
}