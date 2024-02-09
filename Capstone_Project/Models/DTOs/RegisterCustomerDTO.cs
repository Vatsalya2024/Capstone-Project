using System;
namespace Capstone_Project.Models.DTOs
{
    public class RegisterCustomerDTO
    {
        //public string Email { get; set; }
        //public string Password { get; set; }
        //public string UserType { get; set; }
        //public int CustomerID { get; set; }
        //public string Name { get; set; }
        //public DateTime DOB { get; set; }
        //public int Age { get; set; }
        //public long PhoneNumber { get; set; }
        //public string Address { get; set; }
        //public long? AadharNumber { get; set; }
        //public string PANNumber { get; set; }
        //public string Gender { get; set; }


        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string UserType { get; set; } = "Customer";
        public int CustomerID { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime DOB { get; set; }
        public int Age { get; set; }
        public long PhoneNumber { get; set; }
        public string Address { get; set; } = string.Empty;
        public long? AadharNumber { get; set; }
        public string PANNumber { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        // public string Status { get; set; }
    }
}

