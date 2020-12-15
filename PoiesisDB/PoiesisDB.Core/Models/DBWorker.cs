using PoiesisDB.Core.Base;
using PoiesisDB.Core.Data;
using Serilog;

namespace PoiesisDB.Core.Models
{
    public class DBWorker : ObservableObject
    {
        private string _currentProcess;
        public string CurrentProcess
        {
            get { return _currentProcess; }
            set
            {
                _currentProcess = value;
                RaisePropertyChanged("CurrentProcess");
            }
        }

        public bool InitiatePoiesis(DatabaseAttributes databaseAttributes, ILogger logger)
        {
            CurrentProcess = "Creating New Database...";
            bool dbCreated = DataAccess.CreateDatabase(databaseAttributes, logger);

            CurrentProcess = $"Transfering Schema...";
            bool schemaTransfered = DataAccess.TransferSchema(databaseAttributes, logger);

            if (!databaseAttributes.SchemaOnly)
            {
                CurrentProcess = $"Transfering Data...";
                bool dataTransfered = DataAccess.TransferData(databaseAttributes, logger);
            }

            return true;
        }
    }
}
