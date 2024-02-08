using System;
namespace Capstone_Project.Models.DTOs
{
	public class LoanApplicationDTO
	{
        public double LoanAmount { get; set; }
        public string LoanType { get; set; }
        public double Interest { get; set; }
        public int Tenure { get; set; }
        public string Purpose { get; set; }
        public string Status { get; set; }
        public int CustomerID { get; set; }
    }
}

