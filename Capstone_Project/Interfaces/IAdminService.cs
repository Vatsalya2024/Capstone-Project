using System;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Interfaces
{
    public interface IAdminService
    {
        public Task<List<Admin>> GetAllAdmin();
        public Task<Admin> DeleteAdmin(int key);

        public Task<Admin> GetAdmin(int key);
        public Task<Admin> UpdateAdminName(UpdateBankAdminNameDTO updateAdminNameDTO);
    }
}

