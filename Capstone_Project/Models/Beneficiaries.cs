using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone_Project.Models
{
    public class Beneficiaries : IEquatable<Beneficiaries>
    {
        [Key]
        public long AccountNumber { get; set; }
        public string Name { get; set; }
        public string IFSC { get; set; }
        [ForeignKey("IFSC")]
        public Branches? Branches { get; set; }
        public int CustomerID { get; set; }
        [ForeignKey("CustomerID")]
        public Customers? Customers { get; set; }


        public Beneficiaries(long accountNumber, string name, string iFSC, int customerID)
        {
            AccountNumber = accountNumber;
            Name = name;
            IFSC = iFSC;
            CustomerID = customerID;
        }

        public bool Equals(Beneficiaries? other)
        {
            return AccountNumber == other.AccountNumber;
        }
    }
}

