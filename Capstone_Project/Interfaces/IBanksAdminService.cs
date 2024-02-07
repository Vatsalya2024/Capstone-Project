using System;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Interfaces
{
    public interface IBanksAdminService : IBanksUserService
    {
        public Task<Banks> AddBank(Banks item);
        public Task<Banks> UpdateBankName(UpdateBankNameDTO updateBankNameDTO);
        public Task<Banks> DeleteBank(int key);
    }
}

