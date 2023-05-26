using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostAIS.Models
{
    public class Service
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public virtual OperationType OperationType { get; set; }
        public int Code { get; set; }
        public int? ClientId { get; set; }
        public User? Client { get; set; }
    }

    public enum OperationType
    {
        Send,
        Receive,
        Purchase
    }
}
