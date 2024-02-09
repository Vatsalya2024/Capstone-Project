using System;
namespace Capstone_Project.Models.DTOs
{
    public class AccountOpeningDTO
    {

        public string AccountType { get; set; } = string.Empty;
        public string IFSC { get; set; } = string.Empty;
        public int CustomerID { get; set; }

    }
}

