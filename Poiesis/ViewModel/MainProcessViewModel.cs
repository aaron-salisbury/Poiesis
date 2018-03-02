using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using Poiesis.Base;
using Poiesis.Model;
using System.Threading.Tasks;

namespace Poiesis.ViewModel
{
    public class MainProcessViewModel : ViewModelBase, IPageViewModel
    {
        // www.codeproject.com/Questions/1203741/Busy-indicator-is-not-displaying-in-WPF-MVVM

        public string Name { get { return "Main Process"; } }
        public MainProcess MainProcess { get; set; }
        public Application Application { get; set; }
        public RelayCommand ExecuteTaskCommand { get; }
        private Task busyTask;

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set => Set(ref isBusy, value);
        }

        public MainProcessViewModel()
        {
            MainProcess = new MainProcess();
            Application = ((ViewModelLocator)System.Windows.Application.Current.FindResource("Locator")).ApplicationVM.Application;
            ExecuteTaskCommand = new RelayCommand(() => busyTask = InitiateProcess(), () => !isBusy);
        }

        private async Task InitiateProcess()
        {
            System.Windows.Input.Cursor previousCursor = System.Windows.Input.Mouse.OverrideCursor;
            System.Windows.Input.Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

            try
            {
                IsBusy = true;
                bool success = await ExportDataAsync().ConfigureAwait(false);
            }
            // Ensure that no matter what, the busy state is cleared even if there were errors.
            finally
            {
                // Make sure we're on the UI thread.
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    IsBusy = false;
                    ExecuteTaskCommand.RaiseCanExecuteChanged();
                    System.Windows.Input.Mouse.OverrideCursor = previousCursor;
                });
            }
        }

        private Task<bool> ExportDataAsync()
        {
            var tcs = new TaskCompletionSource<bool>();

            Task.Run(() =>
            {
                // Do long running synchronous work here...
                bool processIsSuccessful = MainProcess.InitiatePoiesis(Application.DatabaseAttributes, Application.AppLogger.Logger);

                tcs.SetResult(processIsSuccessful);
            }).ConfigureAwait(false);

            return tcs.Task;
        }
    }
}