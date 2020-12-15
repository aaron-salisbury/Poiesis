using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using PoiesisDB.App.Base;
using PoiesisDB.Core.Models;

namespace PoiesisDB.App.ViewModels
{
    public class ManagerViewModel : BaseViewModel
    {
        public Manager Manager { get; set; }

        public RelayCommand DownloadLogsCommand { get; }

        private BaseViewModel _currentPageViewModel;
        public BaseViewModel CurrentPageViewModel
        {
            
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    RaisePropertyChanged("CurrentPageViewModel");
                }
            }
        }

        public ManagerViewModel()
        {
            Manager = new Manager(AppLogger.Logger);
            DownloadLogsCommand = new RelayCommand(() => AppLogger.OpenLog());
            CurrentPageViewModel = new SelectDatabaseViewModel();

            Messenger.Default.Register<BaseViewModel>(this, "CurrentPageViewModel", message => { CurrentPageViewModel = message; });
        }
    }
}
