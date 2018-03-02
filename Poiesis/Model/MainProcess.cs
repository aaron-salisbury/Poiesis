using GalaSoft.MvvmLight;
using Poiesis.Base;
using Serilog.Core;

namespace Poiesis.Model
{
    public class MainProcess : ObservableObject
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

        public bool InitiatePoiesis(DatabaseAttributes databaseAttributes, Logger logger)
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