using System;
namespace Capstone_Project.Models.DTOs
{
	public class AdminUpdateCustomerContactDTO
	{
        public long? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public long? AadharNumber { get; set; }
    }
}

