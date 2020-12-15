using GalaSoft.MvvmLight.Ioc;

namespace PoiesisDB.App.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<ShellWindowViewModel>();
            SimpleIoc.Default.Register<SettingsAppearanceViewModel>();

            // *** Workflow ***
            SimpleIoc.Default.Register<ManagerViewModel>();
            SimpleIoc.Default.Register<SelectDatabaseViewModel>();
            SimpleIoc.Default.Register<DatabaseAttributesViewModel>();
            SimpleIoc.Default.Register<DBWorkerViewModel>();
        }

        public ShellWindowViewModel ShellWindowViewModel { get { return SimpleIoc.Default.GetInstance<ShellWindowViewModel>(); } }
        public SettingsAppearanceViewModel SettingsAppearanceViewModel { get { return SimpleIoc.Default.GetInstance<SettingsAppearanceViewModel>(); } }

        // *** Workflow ***
        public ManagerViewModel ManagerViewModel { get { return SimpleIoc.Default.GetInstance<ManagerViewModel>(); } }
        public SelectDatabaseViewModel SelectDatabaseViewModel { get { return SimpleIoc.Default.GetInstance<SelectDatabaseViewModel>(); } }
        public DatabaseAttributesViewModel DatabaseAttributesViewModel { get { return SimpleIoc.Default.GetInstance<DatabaseAttributesViewModel>(); } }
        public DBWorkerViewModel DBWorkerViewModel { get { return SimpleIoc.Default.GetInstance<DBWorkerViewModel>(); } }
    }
}
