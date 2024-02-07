using System;
using Capstone_Project.Models;

namespace Capstone_Project.Interfaces
{
    public interface IBanksUserService
    {
        public Task<List<Banks>> GetAllBanks();
        public Task<Banks> GetBank(int key);
    }

}

