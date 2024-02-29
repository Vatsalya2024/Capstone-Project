using System;
namespace Capstone_Project.Models.DTOs
{
    public class BeneficiaryDTO
    {
        
        public long BeneficiaryAccountNumber { get; set; }
        public string? Name { get; set; }
        public string? IFSC { get; set; }
        public int CustomerID { get; set; }
    }

}

