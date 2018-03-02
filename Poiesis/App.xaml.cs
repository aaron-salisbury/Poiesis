using GalaSoft.MvvmLight.Threading;
using System.Windows;

namespace Poiesis
{
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }
    }
}