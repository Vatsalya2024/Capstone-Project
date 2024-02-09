using System;
namespace Capstone_Project.Models.DTOs
{
	public class UpdateCustomerPasswordDTO
	{
        public string Email { get; set; } // The email of the customer whose password is being updated
        public string NewPassword { get; set; } 
    }
}

