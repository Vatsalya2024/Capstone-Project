using System;
namespace Capstone_Project.Models.DTOs
{
    public class UpdateBankNameDTO
    {
        public int BankID { get; set; }
        public string BankName { get; set; } = string.Empty;
    }
}

