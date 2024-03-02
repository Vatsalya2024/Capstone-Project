using System;
namespace Capstone_Project.Models.DTOs
{
    public class BeneficiaryDTO
    {
        
        public long BeneficiaryAccountNumber { get; set; }
        public string Name { get; set; } = string.Empty;
        public string IFSC { get; set; } = string.Empty;
        public int CustomerID { get; set; }
    }

}

