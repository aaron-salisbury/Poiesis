using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Poiesis.Base;
using System;
using System.Deployment.Application;
using System.Reflection;

namespace Poiesis.ViewModel
{
    public class AboutViewModel : ViewModelBase
    {
        private const string COPY_HOLDER = "Aaron Salisbury";

        public string AppVersion { get; set; }
        public string Copyright { get; set; }
        public RelayCommand<IClosable> CloseWindowCommand { get; set; }
        public RelayCommand MITRequestNavigateCommand { get; set; }

        public AboutViewModel()
        {
            AppVersion = $"Version: {GetRunningVersion().ToString()}";
            Copyright = $"© {DateTime.Now.Year} {COPY_HOLDER}";
            CloseWindowCommand = new RelayCommand<IClosable>(CloseWindow);
            MITRequestNavigateCommand = new RelayCommand(MITRequestNavigate);
        }

        private void CloseWindow(IClosable window)
        {
            if (window != null)
            {
                window.Close();
            }
        }

        private void MITRequestNavigate()
        {
            System.Diagnostics.Process.Start("https://opensource.org/licenses/MIT");
        }

        private Version GetRunningVersion()
        {
            try
            {
                return ApplicationDeployment.CurrentDeployment.CurrentVersion;
            }
            catch (Exception)
            {
                return Assembly.GetExecutingAssembly().GetName().Version;
            }
        }
    }
}