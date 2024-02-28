using Capstone_Project.Context;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Capstone_Project.Repositories
{




    public class BeneficiariesRepository : IRepository<int, Beneficiaries>
    {
        private readonly MavericksBankContext _mavericksBankContext;
        private readonly ILogger<BeneficiariesRepository> _logger;

        public BeneficiariesRepository(MavericksBankContext mavericksBankContext, ILogger<BeneficiariesRepository> logger)
        {
            _mavericksBankContext = mavericksBankContext;
            _logger = logger;
        }

        public async Task<Beneficiaries> Add(Beneficiaries item)
        {
            _mavericksBankContext.Beneficiaries.Add(item);
            await _mavericksBankContext.SaveChangesAsync();
            return item;
        }

       

        public async Task<Beneficiaries?> Delete(int key)
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

        public async Task<Beneficiaries?> Get(int key)
        {
            var foundedBeneficiary = await _mavericksBankContext.Beneficiaries.FirstOrDefaultAsync(beneficiary => beneficiary.BeneficiaryID == key);
            return foundedBeneficiary;
        }

        

        public async Task<List<Beneficiaries>?> GetAll()
        {
            var allBeneficiaries = await _mavericksBankContext.Beneficiaries.ToListAsync();
            return allBeneficiaries.Count > 0 ? allBeneficiaries : null;
        }

        public async Task<Beneficiaries> Update(Beneficiaries item)
        {
            _mavericksBankContext.Entry(item).State = EntityState.Modified;
            await _mavericksBankContext.SaveChangesAsync();
            return item;
        }
    }
}
