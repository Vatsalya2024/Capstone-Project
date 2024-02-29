using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone_Project.Models
{
    public class Loans : IEquatable<Loans>
    {
        [Key]
        public int LoanID { get; set; }
        public double LoanAmount { get; set; }
        public string? LoanType { get; set; }
        public double Interest { get; set; }
        public int Tenure { get; set; }
        public string? Purpose { get; set; }
        public string? Status { get; set; }
        public int CustomerID { get; set; }
        [ForeignKey("CustomerID")]
        public Customers? Customers { get; set; }
        public Loans()
        {

        }

        public Loans(int loanID, double loanAmount, string loanType, double interest, int tenure, string purpose, string status, int customerID)
        {
            LoanID = loanID;
            LoanAmount = loanAmount;
            LoanType = loanType;
            Interest = interest;
            Tenure = tenure;
            Purpose = purpose;
            Status = status;
            CustomerID = customerID;
        }

        public bool Equals(Loans? other)
        {
            return LoanID == other?.LoanID;
        }
    }
}

