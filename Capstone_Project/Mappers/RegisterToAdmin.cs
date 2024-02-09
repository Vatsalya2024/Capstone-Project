using System;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Mappers
{
	public class RegisterToAdmin
	{
		Admin admin;
		public RegisterToAdmin(RegisterAdminDTO register)
		{
			admin = new Admin();
			admin.Name = register.Name;
			admin.Email = register.Email;
			admin.Phone = register.Phone;
		}
		public Admin GetAdmin()
		{
			return admin;
		}
	}
}

