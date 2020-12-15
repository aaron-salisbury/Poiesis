using Microsoft.SqlServer.Management.Smo;
using PoiesisDB.Core.Base;

namespace PoiesisDB.Core.Models
{
    public class TransferSchemaObject : ObservableObject
    {
        public bool Complete { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        [System.ComponentModel.Browsable(false)]
        public NamedSmoObject NamedSmoObject { get; set; }
    }
}
