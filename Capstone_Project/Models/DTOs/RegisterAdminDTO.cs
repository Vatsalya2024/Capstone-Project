using System;
namespace Capstone_Project.Models.DTOs
{
	public class RegisterAdminDTO
	{
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string UserType { get; set; } = "Admin";

        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

    }
}

