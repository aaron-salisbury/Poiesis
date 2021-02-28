using GalaSoft.MvvmLight.Ioc;
using Poiesis.App.ViewModels.Workflow;

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

            //*** Workflow ***
            SimpleIoc.Default.Register<WorkflowShellViewModel>();
            SimpleIoc.Default.Register<SourceDBSelectionViewModel>();
            SimpleIoc.Default.Register<DatabaseAttributesViewModel>();
        }

        public ShellWindowViewModel ShellWindowViewModel { get { return SimpleIoc.Default.GetInstance<ShellWindowViewModel>(); } }
        public IntroductionViewModel IntroductionViewModel { get { return SimpleIoc.Default.GetInstance<IntroductionViewModel>(); } }
        public SettingsAppearanceViewModel SettingsAppearanceViewModel { get { return SimpleIoc.Default.GetInstance<SettingsAppearanceViewModel>(); } }
        public LogViewModel LogViewModel { get { return SimpleIoc.Default.GetInstance<LogViewModel>(); } }

        //*** Workflow ***
        public WorkflowShellViewModel WorkflowShellViewModel { get { return SimpleIoc.Default.GetInstance<WorkflowShellViewModel>(); } }
        public SourceDBSelectionViewModel SourceDBSelectionViewModel { get { return SimpleIoc.Default.GetInstance<SourceDBSelectionViewModel>(); } }
        public DatabaseAttributesViewModel DatabaseAttributesViewModel { get { return SimpleIoc.Default.GetInstance<DatabaseAttributesViewModel>(); } }
    }
}
