using System;
using Capstone_Project.Context;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Capstone_Project.Repositories
{
    public class BeneficiariesRepository : IRepository<long, Beneficiaries>
    {
        private readonly MavericksBankContext _mavericksBankContext;
        private readonly ILogger<BranchesRepository> _loggerBranchesRepository;

        public BeneficiariesRepository(MavericksBankContext mavericksBankContext, ILogger<BranchesRepository> loggerBranchesRepository)
        {
            _mavericksBankContext = mavericksBankContext;
            _loggerBranchesRepository = loggerBranchesRepository;
        }

        public async Task<Beneficiaries> Add(Beneficiaries item)
        {
            _mavericksBankContext.Beneficiaries.Add(item);
            await _mavericksBankContext.SaveChangesAsync();
            _loggerBranchesRepository.LogInformation($"Added New Beneficiary : {item.AccountNumber}");
            return item;
        }

        public async Task<Beneficiaries?> Delete(long key)
        {
            var foundedBeneficiary = await Get(key);
            if (foundedBeneficiary == null)
            {
                return null;
            }
            else
            {
                _mavericksBankContext.Beneficiaries.Remove(foundedBeneficiary);
                await _mavericksBankContext.SaveChangesAsync();
                return foundedBeneficiary;
            }
        }

        public async Task<Beneficiaries?> Get(long key)
        {
            var foundedBeneficiary = await _mavericksBankContext.Beneficiaries.FirstOrDefaultAsync(beneficiary => beneficiary.AccountNumber == key);
            if (foundedBeneficiary == null)
            {
                return null;
            }
            else
            {
                return foundedBeneficiary;
            }
        }

        public async Task<List<Beneficiaries>?> GetAll()
        {
            var allBeneficiaries = await _mavericksBankContext.Beneficiaries.ToListAsync();
            if (allBeneficiaries.Count == 0)
            {
                return null;
            }
            else
            {
                return allBeneficiaries;
            }
        }

        public async Task<Beneficiaries> Update(Beneficiaries item)
        {
            _mavericksBankContext.Entry<Beneficiaries>(item).State = EntityState.Modified;
            await _mavericksBankContext.SaveChangesAsync();
            return item;
        }
    }
}

