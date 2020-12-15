using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
using PoiesisDB.App.Base.Extensions;

namespace PoiesisDB.App
{
    /// <summary>
    /// Interaction logic for ShellWindow.xaml
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
