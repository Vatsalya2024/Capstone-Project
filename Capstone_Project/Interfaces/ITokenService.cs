using System;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(LoginUserDTO user);
    }
}

