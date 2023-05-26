using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostAIS.Models
{
    public class PackageToReceive
    {
        public Guid Id { get; set; }
        public string SenderFio { get; set; }
        public string SenderAddress { get; set; }
        public PackageType PackageType { get; set; }
        public string ReceiverTelephoneNumber { get; set; }


    }
}
