using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using PoiesisDB.Core.Models;
using System.Threading.Tasks;

namespace PoiesisDB.App.ViewModels
{
    public class DBWorkerViewModel : BaseViewModel
    {
        public DBWorker DBWorker { get; set; }

        public RelayCommand ExecuteTaskCommand { get; }

        public DBWorkerViewModel()
        {
            ExecuteTaskCommand = new RelayCommand(async () => await InitiateProcessAsync(), () => !IsBusy);
        }

        private async Task InitiateProcessAsync()
        {
            try
            {
                IsBusy = true;
                await ExportDataAsync().ConfigureAwait(false);
            }
            finally
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    IsBusy = false;
                    ExecuteTaskCommand.RaiseCanExecuteChanged();
                });
            }
        }

        private Task<bool> ExportDataAsync()
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            Task.Run(() =>
            {
                // Do long running synchronous work here...
                bool processIsSuccessful = DBWorker.InitiatePoiesis(Locator.ManagerViewModel.Manager.DatabaseAttributes, AppLogger.Logger);

                tcs.SetResult(processIsSuccessful);
            }).ConfigureAwait(false);

            return tcs.Task;
        }
    }
}
