using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlameAndWax.Employee.Models
{
    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhotoLink { get; set; }
        public DateTime DateBirth { get; set; }
        public DateTime HireDate { get; set; }
        public string City { get; set; }
        public string Status { get; set; }
        public string Username { get; set; }        
    }
}