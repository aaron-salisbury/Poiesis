using PoiesisDB.Core.Base;
using PoiesisDB.Core.Data;
using Serilog;
using System.Collections.Generic;

namespace PoiesisDB.Core.Models
{
    public class SelectDatabase : ObservableObject
    {
        public string DataSource { get; set; }
        public string SourceDatabaseName { get; set; }

        public SelectDatabase()
        {
            DataSource = "localhost";
        }

        public List<string> GetLocalDatabaseNames(ILogger logger)
        {
            return DataAccess.GetLocalDatabaseNames(DataSource, logger);
        }
    }
}
