using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostAIS.Models
{
    public class RegistrationCode
    {
        public Guid Id { get; set; }
        public int Code { get; set; }
        public int EmployeeId { get; set; }
        public User? Employee { get; set; }
    }
}
