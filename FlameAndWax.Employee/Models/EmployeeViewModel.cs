using System;
using FlameAndWax.Data.Models;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Employee.Models
{
    public class EmployeeViewModel : LoginViewModel
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhotoLink { get; set; }
        public DateTime DateBirth { get; set; }
        public DateTime HireDate { get; set; }
        public string City { get; set; }
        public EmployeeAccountStatus Status { get; set; }
        public EmployeeViewModel(EmployeeModel employeeModel)
        {
            EmployeeId = employeeModel.EmployeeId;
            FirstName = employeeModel.FirstName;
            LastName = employeeModel.LastName;
            Email = employeeModel.Email;
            PhotoLink = employeeModel.PhotoLink;
            DateBirth = employeeModel.DateBirth;
            HireDate = employeeModel.HireDate;
            City = employeeModel.City;
            Status = employeeModel.Status;
            Username = employeeModel.Username;
        }
        public EmployeeViewModel()
        {
            
        }
    }
}