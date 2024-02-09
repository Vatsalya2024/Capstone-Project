using System;
namespace Capstone_Project.Models.DTOs
{
	public class BeneficiaryDTO
	{
        public long AccountNumber { get; set; }
        public string Name { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public int CustomerId { get; set; }
    }
}

