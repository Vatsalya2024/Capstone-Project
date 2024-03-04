using System;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Services
{
    public class BranchesService : IBranchesAdminService
    {
        private readonly IRepository<string, Branches> _branchesRepository;
        private readonly ILogger<BranchesService> _loggerBranchesService;

        public BranchesService(IRepository<string, Branches> branchesRepository, ILogger<BranchesService> loggerBranchesService)
        {
            _branchesRepository = branchesRepository;
            _loggerBranchesService = loggerBranchesService;
        }

        public async Task<Branches> AddBranch(Branches item)
        {
            _loggerBranchesService.LogInformation("Adding Branch");
            return await _branchesRepository.Add(item);
        }

        public async Task<Branches> DeleteBranch(string key)
        {
            var deletedBranch = await _branchesRepository.Delete(key);
            if (deletedBranch == null)
            {
                throw new NoBranchesFoundException($"Branch IFSC {key} not found");
            }
            _loggerBranchesService.LogInformation("Branch Deleted");
            return deletedBranch;
        }

        public async Task<List<Branches>> GetAllBranches()
        {
            var allBranches = await _branchesRepository.GetAll();
            if (allBranches == null)
            {
                throw new NoBranchesFoundException($"No Branches Data Found");
            }
            _loggerBranchesService.LogInformation("All branches");
            return allBranches;
        }

        public async Task<Branches> GetBranch(string key)
        {
            var foundBranch = await _branchesRepository.Get(key);
            if (foundBranch == null)
            {
                throw new NoBranchesFoundException($"Branch IFSC {key} not found");
            }
            _loggerBranchesService.LogInformation("Branch Found");
            return foundBranch;
        }

        public async Task<Branches> UpdateBranchName(UpdateBranchNameDTO updateBranchNameDTO)
        {
            var foundBranch = await GetBranch(updateBranchNameDTO.IFSCNumber);
            foundBranch.BranchName = updateBranchNameDTO.BranchName;
            var updatedBranch = await _branchesRepository.Update(foundBranch);
            return updatedBranch;
        }
    }
}

