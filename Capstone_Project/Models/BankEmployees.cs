using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone_Project.Models
{
    public class BankEmployees : IEquatable<BankEmployees>
    {
        [Key]
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [ForeignKey("Email")]
        public Validation? Validation { get; set; }

        public BankEmployees(int employeeID, string name, string email)
        {
            EmployeeID = employeeID;
            Name = name;
            Email = email;
        }

        public bool Equals(BankEmployees? other)
        {
            return EmployeeID == other.EmployeeID;
        }
    }
}

