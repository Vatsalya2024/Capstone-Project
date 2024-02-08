using System;
using Capstone_Project.Context;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Capstone_Project.Repositories
{
    public class TransactionsRepository : IRepository<int, Transactions>
    {
        private readonly MavericksBankContext _mavericksBankContext;
        private readonly ILogger<TransactionsRepository> _loggerTransactionsRepository;

        public TransactionsRepository(MavericksBankContext mavericksBankContext, ILogger<TransactionsRepository> loggerTransactionsRepository)
        {
            _mavericksBankContext = mavericksBankContext;
            _loggerTransactionsRepository = loggerTransactionsRepository;
        }

        public async Task<Transactions> Add(Transactions item)
        {
            _mavericksBankContext.Transactions.Add(item);
            await _mavericksBankContext.SaveChangesAsync();
            _loggerTransactionsRepository.LogInformation($"Added New Transaction : {item.TransactionID}");
            return item;
        }

        public async Task<Transactions?> Delete(int key)
        {
            var foundedTransaction = await Get(key);
            if (foundedTransaction == null)
            {
                return null;
            }
            else
            {
                _mavericksBankContext.Transactions.Remove(foundedTransaction);
                await _mavericksBankContext.SaveChangesAsync();
                return foundedTransaction;
            }
        }

        public async Task<Transactions?> Get(int key)
        {
            var foundedTransaction = await _mavericksBankContext.Transactions.FirstOrDefaultAsync(transaction => transaction.TransactionID == key);
            if (foundedTransaction == null)
            {
                return null;
            }
            else
            {
                return foundedTransaction;
            }
        }

        public async Task<List<Transactions>?> GetAll()
        {
            var allTransactions = await _mavericksBankContext.Transactions.ToListAsync();

            if (allTransactions.Count == 0)
            {
                return null;
            }
            else
            {
                return allTransactions;
            }
        }

        public async Task<Transactions> Update(Transactions item)
        {
            _mavericksBankContext.Entry<Transactions>(item).State = EntityState.Modified;
            await _mavericksBankContext.SaveChangesAsync();
            return item;
        }
    }
}

