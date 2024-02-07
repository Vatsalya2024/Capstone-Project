using System;
using System.ComponentModel.DataAnnotations;

namespace Capstone_Project.Models
{
    public class Validation : IEquatable<Validation>
    {
        [Key]
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public string UserType { get; set; }
        public byte[] Key { get; set; }
        public Customers Customers { get; set; }



        public bool Equals(Validation? other)
        {
            if (Email == other.Email && Password == other.Password)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}

