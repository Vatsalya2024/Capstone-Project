using System;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Interfaces
{
	public interface IAdminLoginService
	{
		public Task<LoginUserDTO> Login(LoginUserDTO user);
		public Task<LoginUserDTO> Register(RegisterAdminDTO user);
	}
}

