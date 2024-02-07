using System;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Interfaces
{
    public interface ICustomerLoginService
    {
        public Task<LoginUserDTO> Login(LoginUserDTO user);
        public Task<LoginUserDTO> Register(RegisterCustomerDTO user);
    }
}

