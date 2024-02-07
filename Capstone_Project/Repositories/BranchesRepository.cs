using System;
using Capstone_Project.Context;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Capstone_Project.Repositories
{
    public class BranchesRepository : IRepository<string, Branches>
    {
        private readonly MavericksBankContext _mavericksBankContext;
        private readonly ILogger<BranchesRepository> _loggerBranchesRepository;

        public BranchesRepository(MavericksBankContext mavericksBankContext, ILogger<BranchesRepository> loggerBranchesRepository)
        {
            _mavericksBankContext = mavericksBankContext;
            _loggerBranchesRepository = loggerBranchesRepository;
        }

        public async Task<Branches> Add(Branches item)
        {
            _mavericksBankContext.Branches.Add(item);
            await _mavericksBankContext.SaveChangesAsync();
            _loggerBranchesRepository.LogInformation($"Added New Branch : {item.IFSCNumber}");
            return item;
        }

        public async Task<Branches?> Delete(string key)
        {
            var foundedBranch = await Get(key);
            if (foundedBranch == null)
            {
                return null;
            }
            else
            {
                _mavericksBankContext.Branches.Remove(foundedBranch);
                await _mavericksBankContext.SaveChangesAsync();
                return foundedBranch;
            }
        }

        public async Task<Branches?> Get(string key)
        {
            var foundedBranch = await _mavericksBankContext.Branches.Include(branch => branch.Banks).FirstOrDefaultAsync(branch => branch.IFSCNumber == key);
            if (foundedBranch == null)
            {
                return null;
            }
            else
            {
                return foundedBranch;
            }
        }

        public async Task<List<Branches>?> GetAll()
        {
            var allBranches = await _mavericksBankContext.Branches.Include(branch => branch.Banks).ToListAsync();
            if (allBranches.Count == 0)
            {
                return null;
            }
            else
            {
                return allBranches;
            }
        }

        public async Task<Branches> Update(Branches item)
        {
            _mavericksBankContext.Entry<Branches>(item).State = EntityState.Modified;
            await _mavericksBankContext.SaveChangesAsync();
            return item;
        }
    }
}

