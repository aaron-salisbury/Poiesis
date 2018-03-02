using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Poiesis.Base;
using Poiesis.Model;
using System.Collections.Generic;
using System.Linq;

namespace Poiesis.ViewModel
{
    public class DatabaseAttributesViewModel : ViewModelBase, IPageViewModel
    {
        public string Name { get { return "Database Attributes"; } }
        public DatabaseAttributes DatabaseAttributes { get; set; }
        public RelayCommand RunProcessCommand { get; set; }

        public string NewDatabaseName
        {
            get { return DatabaseAttributes.NewDatabaseName; }
            set
            {
                DatabaseAttributes.NewDatabaseName = value;
                RaisePropertyChanged("NewDatabaseName");
                RaisePropertyChanged("SQLServerSystemFiles");
                CanRunProcess = !string.IsNullOrEmpty(NewDatabaseName);
            }
        }

        private bool _canRunProcess;
        public bool CanRunProcess
        {
            get { return _canRunProcess; }
            set
            {
                if (_canRunProcess != value)
                {
                    _canRunProcess = value;
                    RaisePropertyChanged("CanRunProcess");
                }
            }
        }

        public List<SQLServerSystemFile> SQLServerSystemFiles
        {
            get { return DatabaseAttributes.SQLServerSystemFilesByTypes.Values.ToList(); }
        }

        public string[] TransferTypes
        {
            get { return DatabaseAttributes.TransferTypes; }
        }

        private int _selectedTransferTypeIndex;
        public int SelectedTransferTypeIndex
        {
            get { return _selectedTransferTypeIndex; }
            set
            {
                _selectedTransferTypeIndex = value;
                DatabaseAttributes.SchemaOnly = DatabaseAttributes.TransferTypes[SelectedTransferTypeIndex].Equals("Schema Only");
                RaisePropertyChanged("SelectedTransferTypeIndex");
            }
        }

        public DatabaseAttributesViewModel()
        {
            System.Windows.Input.Cursor previousCursor = System.Windows.Input.Mouse.OverrideCursor;
            System.Windows.Input.Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            DatabaseAttributes = new DatabaseAttributes(((ViewModelLocator)System.Windows.Application.Current.FindResource("Locator")).ApplicationVM.Application);
            System.Windows.Input.Mouse.OverrideCursor = previousCursor;

            RunProcessCommand = new RelayCommand(RunProcess);
            CanRunProcess = !string.IsNullOrEmpty(NewDatabaseName);
            SelectedTransferTypeIndex = 0;
        }

        private void RunProcess()
        {
            ViewModelLocator locator = (ViewModelLocator)System.Windows.Application.Current.FindResource("Locator");

            locator.ApplicationVM.Application.DatabaseAttributes = DatabaseAttributes;

            locator.ApplicationVM.Application.AppLogger.Logger.Information($"Entered new database name of {DatabaseAttributes.NewDatabaseName}.");

            Messenger.Default.Send<IPageViewModel>(locator.MainProcessVM, "CurrentPageViewModel");
        }
    }
}