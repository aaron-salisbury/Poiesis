using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using PoiesisDB.Core.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PoiesisDB.App.ViewModels
{
    public class SelectDatabaseViewModel : BaseViewModel
    {
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

        public string Name => throw new System.NotImplementedException();

        public SelectDatabaseViewModel()
        {
            SelectDatabase = new SelectDatabase();
            EnteredDataSource = SelectDatabase.DataSource;
            //LoadDatabasesCommand = new RelayCommand(LoadDatabaseNames);
            LoadDatabasesCommand = new RelayCommand(async () => await InitiateLoadDatabaseNamesAsync(), () => !IsBusy);
            LocalDatabaseNameSelected = new RelayCommand(SelectLocalDB);
            DisplayDatabaseAttributesCommand = new RelayCommand(DisplayDatabaseAttributes);
        }

        private async Task InitiateLoadDatabaseNamesAsync()
        {
            try
            {
                IsBusy = true;
                await LoadDatabaseNamesAsync().ConfigureAwait(false);
            }
            finally
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    IsBusy = false;
                    LoadDatabasesCommand.RaiseCanExecuteChanged();
                });
            }
        }

        private Task<bool> LoadDatabaseNamesAsync()
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            Task.Run(() =>
            {
                List<string> databaseNames = SelectDatabase.GetLocalDatabaseNames(AppLogger.Logger);

                if (databaseNames != null)
                {
                    LocalDatabaseNames = new ObservableCollection<string>(databaseNames.OrderBy(name => name));
                }

                tcs.SetResult(true);
            }).ConfigureAwait(false);

            return tcs.Task;
        }

        //private void LoadDatabaseNames()
        //{
        //    System.Windows.Input.Cursor previousCursor = System.Windows.Input.Mouse.OverrideCursor;
        //    System.Windows.Input.Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

        //    List<string> databaseNames = SelectDatabase.GetLocalDatabaseNames(AppLogger.Logger);

        //    if (databaseNames != null)
        //    {
        //        LocalDatabaseNames = new ObservableCollection<string>(databaseNames.OrderBy(name => name));
        //    }

        //    System.Windows.Input.Mouse.OverrideCursor = previousCursor;
        //}

        private void SelectLocalDB()
        {
            CanRunProcess = true;
        }

        private void DisplayDatabaseAttributes()
        {
            Locator.ManagerViewModel.Manager.SelectDatabase = SelectDatabase;

            AppLogger.Logger.Information($"Selected database {SelectDatabase.SourceDatabaseName} on server {SelectDatabase.DataSource}.");

            Messenger.Default.Send<BaseViewModel>(Locator.DatabaseAttributesViewModel, "CurrentPageViewModel");
        }
    }
}