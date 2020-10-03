using GalaSoft.MvvmLight.Ioc;

namespace Poiesis.ViewModel
{
    public class ViewModelLocator
    {
        // stackoverflow.com/questions/16913077/accessing-properties-in-other-viewmodels-in-mvvm-light

        static ViewModelLocator()
        {
            //ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<ApplicationViewModel>();
            SimpleIoc.Default.Register<SelectDatabaseViewModel>();
            SimpleIoc.Default.Register<DatabaseAttributesViewModel>();
            SimpleIoc.Default.Register<AboutViewModel>();
            SimpleIoc.Default.Register<MainProcessViewModel>();
        }

        public ApplicationViewModel ApplicationVM { get { return SimpleIoc.Default.GetInstance <ApplicationViewModel>(); } }

        public SelectDatabaseViewModel SelectDatabaseVM { get { return SimpleIoc.Default.GetInstance <SelectDatabaseViewModel>(); } }

        public DatabaseAttributesViewModel DatabaseAttributesVM { get { return SimpleIoc.Default.GetInstance <DatabaseAttributesViewModel>(); } }

        public MainProcessViewModel MainProcessVM { get { return SimpleIoc.Default.GetInstance<MainProcessViewModel>(); } }

        public AboutViewModel AboutVM { get { return SimpleIoc.Default.GetInstance <AboutViewModel>(); } }
    }
}