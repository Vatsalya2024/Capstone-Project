using System;
using Capstone_Project.Models;

namespace Capstone_Project.Interfaces
{
    public interface IBranchesUserService
    {
        public Task<List<Branches>> GetAllBranches();
        public Task<Branches> GetBranch(string key);
    }
}

