using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone_Project.Models.DTOs
{
	public class RegisterBankEmployeeDTO
	{
        
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string UserType { get; set; } = "BankEmployee";
       
        public string Name { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}

