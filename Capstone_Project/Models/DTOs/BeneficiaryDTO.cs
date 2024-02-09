using System;
namespace Capstone_Project.Models.DTOs
{
	public class BeneficiaryDTO
	{
        public long AccountNumber { get; set; }
        public string Name { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public int CustomerId { get; set; }
    }
}

