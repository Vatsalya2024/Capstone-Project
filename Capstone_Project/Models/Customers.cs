using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone_Project.Models
{
    public class Customers : IEquatable<Customers>
    {
        [Key]
        public int CustomerID { get; set; }
        public string? Name { get; set; }
        public DateTime DOB { get; set; }
        public int Age { get; set; }
        public long PhoneNumber { get; set; }
        public string? Address { get; set; }
        public long? AadharNumber { get; set; }
        public string? PANNumber { get; set; }
        public string? Gender { get; set; }
        public string? Email { get; set; }
        [ForeignKey("Email")]
        public Validation? Validation { get; set; }

        public ICollection<Accounts>? Accounts { get; set; }
        public ICollection<Beneficiaries>? Beneficiaries { get; set; }
        public ICollection<Loans>? Loans { get; set; }

        public Customers(int customerID, string name, DateTime dOB, int age, long phoneNumber, string address, long? aadharNumber, string pANNumber, string gender, string email)
        {
            CustomerID = customerID;
            Name = name??"";
            DOB = dOB;
            Age = age;
            PhoneNumber = phoneNumber;
            Address = address ?? ""; ;
            AadharNumber = aadharNumber;
            PANNumber = pANNumber ?? ""; ;
            Gender = gender ?? ""; ;
            Email = email ?? ""; ;
        }

        public Customers()
        {
        }

        public bool Equals(Customers? other)
        {
            return CustomerID == other?.CustomerID;
        }
    }
}

