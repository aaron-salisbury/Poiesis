using GalaSoft.MvvmLight;
using Poiesis.Base;

namespace Poiesis.Model
{
    public class Application : ObservableObject
    {
        public SelectDatabase SelectDatabase { get; set; }
        public DatabaseAttributes DatabaseAttributes { get; set; }
        public AppLogger AppLogger { get; set; }

        public Application()
        {
            AppLogger = new AppLogger();
        }
    }
}