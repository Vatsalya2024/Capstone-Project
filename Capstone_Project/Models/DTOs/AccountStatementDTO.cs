using System;
namespace Capstone_Project.Models.DTOs
{
    public class AccountStatementDTO
    {
        public string CreditScore { get; set; } = string.Empty;
        public double TotalDebit { get; set; }
        public double TotalCredit { get; set; }
    }
}

