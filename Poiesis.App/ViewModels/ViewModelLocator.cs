using GalaSoft.MvvmLight.Ioc;

namespace Poiesis.App.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<ShellWindowViewModel>();
            SimpleIoc.Default.Register<IntroductionViewModel>();
            SimpleIoc.Default.Register<SettingsAppearanceViewModel>();
            SimpleIoc.Default.Register<LogViewModel>();
        }

        public ShellWindowViewModel ShellWindowViewModel { get { return SimpleIoc.Default.GetInstance<ShellWindowViewModel>(); } }
        public IntroductionViewModel IntroductionViewModel { get { return SimpleIoc.Default.GetInstance<IntroductionViewModel>(); } }
        public SettingsAppearanceViewModel SettingsAppearanceViewModel { get { return SimpleIoc.Default.GetInstance<SettingsAppearanceViewModel>(); } }
        public LogViewModel LogViewModel { get { return SimpleIoc.Default.GetInstance<LogViewModel>(); } }
    }
}
