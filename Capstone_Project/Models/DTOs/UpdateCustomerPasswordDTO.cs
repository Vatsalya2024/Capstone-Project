using System;
namespace Capstone_Project.Models.DTOs
{
	public class UpdateCustomerPasswordDTO
	{
        public string Email { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}

