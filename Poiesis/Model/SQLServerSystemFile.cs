using Serilog.Core;
using System;
using System.Data.SqlClient;

namespace Poiesis.Model
{
    public class SQLServerSystemFile
    {
        public enum Types { Data, Log };

        public Types Type { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string File { get; set; }
        public string Size { get; set; }
        public string MaxSize { get; set; }
        public string Growth { get; set; }

        public static SQLServerSystemFile CreateFromSqlDataReader(SqlDataReader sqlDataReader, Types type, Logger logger)
        {
            try
            {
                SQLServerSystemFile systemFile = new SQLServerSystemFile()
                {
                    Type = type,
                    Name = sqlDataReader[5].ToString(),
                    Path = System.IO.Path.GetDirectoryName(sqlDataReader[6].ToString()),
                    File = System.IO.Path.GetFileName(sqlDataReader[6].ToString()),
                    Size = sqlDataReader[9].ToString(),
                    MaxSize = sqlDataReader[10].ToString(),
                    Growth = sqlDataReader[11].ToString()
                };

                bool isPercentGrowth = Convert.ToBoolean(sqlDataReader[15].ToString());

                if (isPercentGrowth)
                {
                    systemFile.Growth = $"{systemFile.Growth}%";
                }
                else
                {
                    int growth = Convert.ToInt32(systemFile.Growth) / 128;
                    systemFile.Growth = $"{growth}MB";
                }

                int size = Convert.ToInt32(systemFile.Size) / 128;
                systemFile.Size = $"{size}MB";

                if (string.Equals(systemFile.MaxSize, "-1"))
                {
                    systemFile.MaxSize = "UNLIMITED";
                }
                else
                {
                    int maxSize = Convert.ToInt32(systemFile.MaxSize) / 128;
                    systemFile.MaxSize = $"{maxSize}MB";
                }

                return systemFile;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                return null;
            }
        }
    }
}