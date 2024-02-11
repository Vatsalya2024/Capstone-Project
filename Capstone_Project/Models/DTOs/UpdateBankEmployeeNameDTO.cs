using System;
namespace Capstone_Project.Models.DTOs
{
    public class UpdateBankEmployeeNameDTO
    {
        public int EmployeeID { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}

