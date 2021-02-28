using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Poiesis.Core;

namespace Poiesis.App.ViewModels.Workflow
{
    public class SourceDBSelectionViewModel : BaseViewModel
    {
        public Manager Manager { get => Locator.WorkflowShellViewModel.Manager; }

        public RelayCommand LoadDatabasesCommand { get; set; }
        public RelayCommand DisplayDatabaseAttributesCommand { get; set; }

        private string _selectedLocalDatabaseName;
        public string SelectedLocalDatabaseName
        {
            get { return _selectedLocalDatabaseName; }
            set
            {
                _selectedLocalDatabaseName = value;
                RaisePropertyChanged();
                Manager.SourceDatabase.Name = SelectedLocalDatabaseName;
                CanRunProcess = !string.IsNullOrEmpty(value);
            }
        }

        private bool _canRunProcess;
        public bool CanRunProcess
        {
            get { return _canRunProcess; }
            set
            {
                _canRunProcess = value;
                RaisePropertyChanged();
            }
        }

        public SourceDBSelectionViewModel()
        {
            LoadDatabasesCommand = new RelayCommand(async () => await InitiateProcessAsync(Manager.SetLocalDatabaseNames, LoadDatabasesCommand), () => !IsBusy);
            DisplayDatabaseAttributesCommand = new RelayCommand(async () => await NavigateToAttributesAsync(), () => !IsBusy);
        }

        private async System.Threading.Tasks.Task NavigateToAttributesAsync()
        {
            await InitiateProcessAsync(Manager.SetSourceDatabaseSQLServerSystemFiles, DisplayDatabaseAttributesCommand);

            await InitiateProcessAsync(Manager.SetNewDatabase, DisplayDatabaseAttributesCommand);

            Messenger.Default.Send<BaseViewModel>(Locator.DatabaseAttributesViewModel, "CurrentPageViewModel");
        }
    }
}
