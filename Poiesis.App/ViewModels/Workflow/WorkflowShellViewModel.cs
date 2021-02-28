using Poiesis.Core;

namespace Poiesis.App.ViewModels.Workflow
{
    public class WorkflowShellViewModel : BaseViewModel
    {
        public Manager Manager { get; set; }

        private BaseViewModel _currentPageViewModel;
        public BaseViewModel CurrentPageViewModel
        {
            get => _currentPageViewModel;
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    RaisePropertyChanged(nameof(CurrentPageViewModel));
                }
            }
        }

        private BaseViewModel _logViewModel;
        public BaseViewModel LogViewModel
        {
            get => _logViewModel;
            set
            {
                if (_logViewModel != value)
                {
                    _logViewModel = value;
                    RaisePropertyChanged(nameof(LogViewModel));
                }
            }
        }

        public WorkflowShellViewModel()
        {
            Manager = new Manager(AppLogger);
            CurrentPageViewModel = new SourceDBSelectionViewModel();
            LogViewModel = new LogViewModel();
        }
    }
}
