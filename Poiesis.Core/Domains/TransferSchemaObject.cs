using Microsoft.SqlServer.Management.Smo;
using System.ComponentModel;

namespace Poiesis.Core.Domains
{
    public class TransferSchemaObject
    {
        public bool Complete { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        [Browsable(false)]
        public NamedSmoObject NamedSmoObject { get; set; }
    }
}
