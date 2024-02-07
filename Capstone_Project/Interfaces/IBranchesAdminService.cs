using System;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Interfaces
{
    public interface IBranchesAdminService : IBranchesUserService
    {
        public Task<Branches> AddBranch(Branches item);
        public Task<Branches> UpdateBranchName(UpdateBranchNameDTO updateBranchNameDTO);
        public Task<Branches> DeleteBranch(string key);
    }
}

