using System;
namespace Capstone_Project.Models.DTOs
{
	public class AdminUpdateCustomerDetailsDTO
	{
        public DateTime? DOB { get; set; }
        public int? Age { get; set; }
        public string? PANNumber { get; set; }
        public string? Gender { get; set; }
    }
}

