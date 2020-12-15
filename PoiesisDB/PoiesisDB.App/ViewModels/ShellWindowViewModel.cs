using FirstFloor.ModernUI.Presentation;
using GalaSoft.MvvmLight;
using PoiesisDB.App.Properties;
using PoiesisDB.Core.Base;
using System;
using System.ComponentModel;
using System.Threading;

namespace PoiesisDB.App.ViewModels
{
    public class ShellWindowViewModel : ViewModelBase
    {
        private const string UNIQUE_APP_KEY = "ade09ef8-156a-4d31-bf70-5650a19f3554";

        private static Link _settingsUri = new Link { DisplayName = "Settings", Source = new Uri("/Views/Settings.xaml", UriKind.Relative) };

        public AppLogger AppLogger { get; set; }

        private string _helpURL;
        public string HelpURL
        {
            get => _helpURL;
            set
            {
                Set(ref _helpURL, value);
                HelpUri = new Link { DisplayName = "Help", Source = new Uri(HelpURL, UriKind.Absolute) };
            }
        }

        private Link _helpUri;
        public Link HelpUri
        {
            get => _helpUri;
            set
            {
                Set(ref _helpUri, value);
                TitleLinks = new LinkCollection { _settingsUri, HelpUri };
            }
        }

        private LinkCollection _titleLinks = new LinkCollection();
        public LinkCollection TitleLinks
        {
            get => _titleLinks;
            set => Set(ref _titleLinks, value);
        }

        public string Title
        {
            get => Settings.Default.ApplicationFriendlyName;
        }

        public ShellWindowViewModel()
        {
            using (Mutex mutex = new Mutex(true, UNIQUE_APP_KEY, out bool mutexIsNew))
            {
                if (!mutexIsNew)
                {
                    System.Windows.Application.Current.Shutdown();
                }
            }

            AppLogger = new AppLogger();
            AppLogger.Logger.Information("Launched application.");

            HelpURL = Properties.Settings.Default.DefaultHelpURL;

            System.Windows.Application.Current.MainWindow.Closing += new CancelEventHandler(ShellWindow_Closing);
        }

        void ShellWindow_Closing(object sender, CancelEventArgs e)
        {
            GalaSoft.MvvmLight.Threading.DispatcherHelper.Reset();
        }
    }
}
