
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone_Project.Models
{
    public class Transactions : IEquatable<Transactions>
    {
        [Key]
        public int TransactionID { get; set; }
        public double Amount { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public string Description { get; set; } = string.Empty;
        public string TransactionType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        
        public long SourceAccountNumber { get; set; }
        [ForeignKey("SourceAccountNumber")]
        public Accounts? Accounts { get; set; }

        
        public long? DestinationAccountNumber { get; set; }
        [ForeignKey("DestinationAccountNumber")]
        public Accounts? DestinationAccount { get; set; }


        public int? BeneficiaryID { get; set; } 
        [ForeignKey("BeneficiaryID")] 
        public Beneficiaries? Beneficiary { get; set; }

        public Transactions()
        {

        }

        public Transactions(int transactionID, double amount, DateTime transactionDate, string description, string transactionType, string status, long sourceAccountNumber, long? destinationAccountNumber, int? beneficiaryId)
        {
            TransactionID = transactionID;
            Amount = amount;
            TransactionDate = transactionDate;
            Description = description ?? string.Empty;
            TransactionType = transactionType ?? string.Empty;
            Status = status ?? string.Empty;
            SourceAccountNumber = sourceAccountNumber;
            DestinationAccountNumber = destinationAccountNumber;
            BeneficiaryID = beneficiaryId;
        }

        public bool Equals(Transactions? other)
        {
            return TransactionID == other?.TransactionID;
        }
    }
}
