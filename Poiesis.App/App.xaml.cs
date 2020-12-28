using System.Windows;

namespace Poiesis.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            GalaSoft.MvvmLight.Threading.DispatcherHelper.Initialize();
        }
    }
}
