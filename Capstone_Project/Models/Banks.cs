using System;
using System.ComponentModel.DataAnnotations;

namespace Capstone_Project.Models
{
    public class Banks : IEquatable<Banks>
    {
        [Key]
        public int BankID { get; set; }
        public string BankName { get; set; }
        
        public Banks(int bankID, string bankName)
        {
            BankID = bankID;
            BankName = bankName;
        }

        public bool Equals(Banks? other)
        {
            return BankID == other?.BankID;
        }
    }
}

