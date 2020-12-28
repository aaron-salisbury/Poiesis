using Poiesis.App.Properties;

namespace Poiesis.App.ViewModels
{
    public class IntroductionViewModel : BaseViewModel
    {
        public string Title
        {
            get => Settings.Default.ApplicationFriendlyName;
        }
    }
}
