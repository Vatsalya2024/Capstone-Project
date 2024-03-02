using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone_Project.Models
{
    public class Accounts : IEquatable<Accounts>
    {
        [Key]
        public long AccountNumber { get; set; }
        public double Balance { get; set; }
        public string? AccountType { get; set; }
        public string? Status { get; set; }
        public string? IFSC { get; set; }
        [ForeignKey("IFSC")]
        public Branches? Branches { get; set; }
        public int CustomerID { get; set; }
        [ForeignKey("CustomerID")]
        public Customers? Customers { get; set; }

        public ICollection<Transactions>? SourceTransaction { get; set; }
        public ICollection<Transactions>? Transfers { get; set; }

        public Accounts()
        {
            AccountNumber = GenerateAccountNumber();
        }
        public Accounts(double balance, string accountType, string status, string iFSC, int customerID)
        {
            AccountNumber = GenerateAccountNumber();
            Balance = balance;
            AccountType = accountType;
            Status = status;
            IFSC = iFSC;
            CustomerID = customerID;
        }

        public bool Equals(Accounts? other)
        {
            return AccountNumber == this.AccountNumber;
        }

        private long GenerateAccountNumber()
        {
            Random rnd = new Random();
            int randomPart = rnd.Next(10000, 99999);
            long accountNumber = long.Parse("11133" + randomPart.ToString());
            return accountNumber;
        }
    }
}


