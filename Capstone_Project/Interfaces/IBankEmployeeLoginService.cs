using System;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Interfaces
{
	public interface IBankEmployeeLoginService
	{
        Task<LoginUserDTO> Login(LoginUserDTO employee);
        Task<LoginUserDTO> Register(RegisterBankEmployeeDTO employee);
        Task<BankEmployees?> GetBankEmployeeInfoByEmail(string email);
    }
}

