using System;
using Capstone_Project.Context;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Capstone_Project.Repositories
{
    public class LoansRepository : IRepository<int, Loans>
    {
        private readonly MavericksBankContext _mavericksBankContext;
        private readonly ILogger<LoansRepository> _loggerLoansRepository;

        public LoansRepository(MavericksBankContext mavericksBankContext, ILogger<LoansRepository> loggerLoansRepository)
        {
            _mavericksBankContext = mavericksBankContext;
            _loggerLoansRepository = loggerLoansRepository;
        }

        public async Task<Loans> Add(Loans item)
        {
            _mavericksBankContext.Loans.Add(item);
            await _mavericksBankContext.SaveChangesAsync();
            _loggerLoansRepository.LogInformation($"Added New Loan : {item.LoanID}");
            return item;
        }

        public async Task<Loans?> Delete(int key)
        {
            var foundedLoan = await Get(key);
            if (foundedLoan == null)
            {
                return null;
            }
            else
            {
                _mavericksBankContext.Loans.Remove(foundedLoan);
                await _mavericksBankContext.SaveChangesAsync();
                return foundedLoan;
            }
        }

        public async Task<Loans?> Get(int key)
        {
            var foundedLoan = await _mavericksBankContext.Loans
                 .Include(l => l.Customers)
                  .ThenInclude(c => c!.Accounts)
                .FirstOrDefaultAsync(loan => loan.LoanID == key);
            if (foundedLoan == null)
            {
                return null;
            }
            else
            {
                return foundedLoan;
            }
        }

        public async Task<List<Loans>?> GetAll()
        {
            var allLoans = await _mavericksBankContext.Loans.ToListAsync();
            if (allLoans.Count == 0)
            {
                return null;
            }
            else
            {
                return allLoans;
            }
        }

        public async Task<Loans> Update(Loans item)
        {
            _mavericksBankContext.Entry<Loans>(item).State = EntityState.Modified;
            await _mavericksBankContext.SaveChangesAsync();
            return item;
        }
    }
}

