using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Poiesis.Base;
using Poiesis.Model;
using Poiesis.View;
using System.Collections.Concurrent;
using System.Threading;

namespace Poiesis.ViewModel
{
    public class ApplicationViewModel : ViewModelBase
    {
        private const string UNIQUE_APP_KEY = "ade09ef8-156a-4d31-bf70-5650a19f3554";

        public Application Application { get; set; }
        public RelayCommand AboutMenuCommand { get; set; }
        public RelayCommand LogMenuCommand { get; set; }

        private IPageViewModel _currentPageViewModel;
        public IPageViewModel CurrentPageViewModel
        {
            // Simple Navigation: rachel53461.wordpress.com/2011/12/18/navigation-with-mvvm-2/
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

        public ConcurrentQueue<string> Events
        {
            get { return Application.AppLogger.InMemorySink.Events; }
            set
            {
                Events = value;
                RaisePropertyChanged("Events");
            }
        }

        public ApplicationViewModel()
        {
            using (Mutex mutex = new Mutex(true, UNIQUE_APP_KEY, out bool mutexIsNew))
            {
                if (!mutexIsNew)
                {
                    System.Windows.Application.Current.Shutdown();
                }
            }

            Application = new Application();
            AboutMenuCommand = new RelayCommand(OpenAbout);
            LogMenuCommand = new RelayCommand(OpenLog);
            CurrentPageViewModel = new SelectDatabaseViewModel();

            Messenger.Default.Register<IPageViewModel>(this, "CurrentPageViewModel", message => { CurrentPageViewModel = message; });
        }

        private void OpenAbout()
        {
            //TODO: I hate interacting with a view from within a view-model.
            AboutView aboutView = new AboutView();
            aboutView.ShowDialog();
        }

        private void OpenLog()
        {
            Application.AppLogger.OpenLog();
        }
    }
}