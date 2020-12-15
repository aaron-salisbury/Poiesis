using PoiesisDB.Core.Base;
using Serilog;

namespace PoiesisDB.Core.Models
{
    public class Manager : ObservableObject
    {
        public SelectDatabase SelectDatabase { get; set; }
        public DatabaseAttributes DatabaseAttributes { get; set; }
        public ILogger Logger { get; set; }

        public Manager(ILogger logger)
        {
            Logger = logger;
        }
    }
}
