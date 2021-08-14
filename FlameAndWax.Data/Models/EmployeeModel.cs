using System;

namespace FlameAndWax.Data.Models
{
    public class EmployeeModel
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhotoLink { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime HireDate { get; set; }
        public string City { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
