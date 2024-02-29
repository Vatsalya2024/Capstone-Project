using System;
using System.ComponentModel.DataAnnotations;

namespace Capstone_Project.Models
{
    public class Validation : IEquatable<Validation>
    {


        [Key]
        public string? Email { get; set; } 
        public byte[] Password { get; set; } = Array.Empty<byte>();
        public string? UserType { get; set; } 
        public byte[] Key { get; set; } = Array.Empty<byte>();
        public string? Status { get; set; } 
        public Customers? Customers { get; set; }
        public BankEmployees? BankEmployees { get; set; }
        public Admin? Admin { get; set; }



        public bool Equals(Validation? other)
        {
            if (other == null)
            {
                return false; 
            }

            return Email == other.Email && Password == other.Password;
        }

    }
}

