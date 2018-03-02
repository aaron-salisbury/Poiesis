using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace Poiesis.ViewModel
{
    public class ViewModelLocator
    {
        // stackoverflow.com/questions/16913077/accessing-properties-in-other-viewmodels-in-mvvm-light

        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<ApplicationViewModel>();
            SimpleIoc.Default.Register<SelectDatabaseViewModel>();
            SimpleIoc.Default.Register<DatabaseAttributesViewModel>();
            SimpleIoc.Default.Register<AboutViewModel>();
            SimpleIoc.Default.Register<MainProcessViewModel>();
        }

        public ApplicationViewModel ApplicationVM { get { return ServiceLocator.Current.GetInstance <ApplicationViewModel>(); } }

        public SelectDatabaseViewModel SelectDatabaseVM { get { return ServiceLocator.Current.GetInstance <SelectDatabaseViewModel>(); } }

        public DatabaseAttributesViewModel DatabaseAttributesVM { get { return ServiceLocator.Current.GetInstance <DatabaseAttributesViewModel>(); } }

        public MainProcessViewModel MainProcessVM { get { return ServiceLocator.Current.GetInstance<MainProcessViewModel>(); } }

        public AboutViewModel AboutVM { get { return ServiceLocator.Current.GetInstance <AboutViewModel>(); } }
    }
}