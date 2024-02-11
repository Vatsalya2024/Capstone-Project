using System;
namespace Capstone_Project.Models.DTOs
{
	public class LoanApplicationDTO
	{
        public double LoanAmount { get; set; }
        public string LoanType { get; set; } = string.Empty;
        public double Interest { get; set; }
        public int Tenure { get; set; }
        public string Purpose { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public int CustomerID { get; set; }
    }
}

