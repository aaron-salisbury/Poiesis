using System;

namespace Poiesis.Core.Domains
{
    public class TransferDataObject
    {
        public bool Complete { get; set; }

        public string Table { get; set; }

        public string SqlCommand { get; set; }
    }
}
