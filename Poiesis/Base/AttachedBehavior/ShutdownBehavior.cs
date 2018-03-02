using System.Windows;
using System.Windows.Controls;

namespace Poiesis.Base.AttachedBehavior
{
    public class ShutdownBehavior : DependencyObject
    {
        public static bool? GetForceShutdown(DependencyObject obj)
        {
            return (bool?)obj.GetValue(ForceShutdownProperty);
        }

        public static void SetForceShutdown(DependencyObject obj, bool? value)
        {
            obj.SetValue(ForceShutdownProperty, value);
        }

        public static readonly DependencyProperty ForceShutdownProperty =
            DependencyProperty.RegisterAttached("ForceShutdown", typeof(bool?),
            typeof(ShutdownBehavior),
            new UIPropertyMetadata(null, (sender, e) => OnPropertyChangedCallBack(sender, e)));

        private static void OnPropertyChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MenuItem mnu = sender as MenuItem;
            if (mnu == null)
            {
                return;
            }

            mnu.Click -= new RoutedEventHandler(mnu_Click);
            mnu.Click += new RoutedEventHandler(mnu_Click);
        }

        static void mnu_Click(object sender, RoutedEventArgs e)
        {
            if (GetForceShutdown(sender as DependencyObject).Value == true)
            {
                Application.Current.Shutdown();
            }

            else if (GetForceShutdown(sender as DependencyObject).Value == false)
            {
                if (MessageBox.Show("Are you sure you want to exit the application?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Application.Current.Shutdown();
                }
            }
        }
    }
}