using GalaSoft.MvvmLight;
using Poiesis.Base;
using System.Collections.Concurrent;
using System.IO;

namespace Poiesis.Model
{
    public class Application : ObservableObject
    {
        public SelectDatabase SelectDatabase { get; set; }
        public DatabaseAttributes DatabaseAttributes { get; set; }
        public AppLogger AppLogger { get; set; }

        public ConcurrentQueue<string> Events
        {
            get { return AppLogger.InMemorySink.Events; }
        }

        public Application()
        {
            AppLogger = new AppLogger();
        }
    }
}