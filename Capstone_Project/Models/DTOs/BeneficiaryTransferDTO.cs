using System;
namespace Capstone_Project.Models.DTOs
{
	public class BeneficiaryTransferDTO
	{
        public int BeneficiaryID { get; set; }
        public long SourceAccountNumber { get; set; }
        public long BeneficiaryAccountNumber { get; set; }
        public float Amount { get; set; }
    }
}

