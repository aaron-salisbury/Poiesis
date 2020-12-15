using PoiesisDB.Core.Base;

namespace PoiesisDB.Core.Models
{
    public class TransferDataObject : ObservableObject
    {
        public bool Complete { get; set; }
        public string Table { get; set; }
        public string SqlCommand { get; set; }
    }
}
