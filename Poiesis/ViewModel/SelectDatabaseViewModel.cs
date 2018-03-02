using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Poiesis.Base;
using Poiesis.Model;
using Serilog.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Poiesis.ViewModel
{
    public class SelectDatabaseViewModel : ViewModelBase, IPageViewModel
    {
        public string Name { get { return "Select Database"; } }
        public RelayCommand LoadDatabasesCommand { get; set; }
        public RelayCommand LocalDatabaseNameSelected { get; set; }
        public RelayCommand DisplayDatabaseAttributesCommand { get; set; }

        private string _enteredDataSource;
        public string EnteredDataSource
        {
            get { return _enteredDataSource; }
            set
            {
                _enteredDataSource = value;
                SelectDatabase.DataSource = EnteredDataSource;
                RaisePropertyChanged("EnteredDataSource");
            }
        }

        private SelectDatabase _selectDatabase;
        public SelectDatabase SelectDatabase
        {
            get { return _selectDatabase; }
            set
            {
                _selectDatabase = value;
                RaisePropertyChanged("SelectDatabase");
            }
        }

        private ObservableCollection<string> _localDatabaseNames;
        public ObservableCollection<string> LocalDatabaseNames
        {
            get { return _localDatabaseNames; }
            set
            {
                _localDatabaseNames = value;
                RaisePropertyChanged("LocalDatabaseNames");
            }
        }

        private bool _canRunProcess;
        public bool CanRunProcess
        {
            get { return _canRunProcess; }
            set
            {
                _canRunProcess = value;
                RaisePropertyChanged("CanRunProcess");
            }
        }

        private string _selectedLocalDatabaseName;
        public string SelectedLocalDatabaseName
        {
            get { return _selectedLocalDatabaseName; }
            set
            {
                _selectedLocalDatabaseName = value;
                SelectDatabase.SourceDatabaseName = SelectedLocalDatabaseName;
            }
        }

        public SelectDatabaseViewModel()
        {
            SelectDatabase = new SelectDatabase();
            EnteredDataSource = SelectDatabase.DataSource;
            LoadDatabasesCommand = new RelayCommand(LoadDatabaseNames);
            LocalDatabaseNameSelected = new RelayCommand(SelectLocalDB);
            DisplayDatabaseAttributesCommand = new RelayCommand(DisplayDatabaseAttributes);
        }

        private void LoadDatabaseNames()
        {
            System.Windows.Input.Cursor previousCursor = System.Windows.Input.Mouse.OverrideCursor;
            System.Windows.Input.Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

            Logger logger = ((ViewModelLocator)System.Windows.Application.Current.FindResource("Locator")).ApplicationVM.Application.AppLogger.Logger;
            List<string> databaseNames = SelectDatabase.GetLocalDatabaseNames(logger);

            if (databaseNames != null)
            {
                LocalDatabaseNames = new ObservableCollection<string>(databaseNames.OrderBy(name => name));
            }

            System.Windows.Input.Mouse.OverrideCursor = previousCursor;
        }

        private void SelectLocalDB()
        {
            CanRunProcess = true;
        }

        private void DisplayDatabaseAttributes()
        {
            ViewModelLocator locator = (ViewModelLocator)System.Windows.Application.Current.FindResource("Locator");

            locator.ApplicationVM.Application.SelectDatabase = SelectDatabase;

            locator.ApplicationVM.Application.AppLogger.Logger.Information($"Selected database {SelectDatabase.SourceDatabaseName} on server {SelectDatabase.DataSource}.");

            Messenger.Default.Send<IPageViewModel>(locator.DatabaseAttributesVM, "CurrentPageViewModel");
        }
    }
}