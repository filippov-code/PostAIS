using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostAIS.Models
{
    public class User
    {
        public int Id { get; set; }
        public string LastName { get; set; }//фамилия
        public string FirstName { get; set; }//имя
        public string MiddleName { get; set; }//отчество
        public DateTime Birthday { get; set; }
        public string PassportSeries { get; set; }
        public string PassportNumber { get; set; }
        public string Address { get; set; }
        public string TelephoneNumber { get; set; }
        public string Password { get; set; }
        public DateTime Created { get; set; }
        public int? RegisteringEmployeeId { get; set; }
        public User? RegisteringEmployee { get; set; }
        public UserRole Role { get; set; }

        public override string ToString()
        {
            return $"{LastName} {FirstName} {MiddleName}";
        }
    }

    public enum UserRole
    {
        Client,
        Employee,
        Administrator
    }
}
