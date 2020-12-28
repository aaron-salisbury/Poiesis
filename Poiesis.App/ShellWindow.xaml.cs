using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
using Poiesis.App.Base.Extensions;

namespace Poiesis.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ShellWindow : ModernWindow
    {
        public ShellWindow()
        {
            InitializeComponent();

            AppearanceManager.Current.LoadAppearancesFromSettings(Properties.Settings.Default);
        }
    }
}
