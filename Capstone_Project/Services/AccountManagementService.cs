using System;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Services
{
    public class AccountManagementService : IAccountManagementService
    {
        private readonly IRepository<long, Accounts> _accountsRepository;
        private readonly IRepository<int, Transactions> _transactionsRepository;

        public AccountManagementService(IRepository<long, Accounts> accountsRepository, IRepository<int, Transactions> transactionsRepository)
        {
            _accountsRepository = accountsRepository;
            _transactionsRepository = transactionsRepository;
        }


        public async Task<bool> CloseAccount(long accountNumber)
        {
            var account = await _accountsRepository.Get(accountNumber);

            if (account != null)
            {

                account.Status = "PendingDeletion";
                await _accountsRepository.Update(account);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Accounts> GetAccountDetails(long accountNumber)
        {
            var account = await _accountsRepository.Get(accountNumber);
            return account;
        }

        public async Task<List<Accounts>> GetAllAccountsByCustomerId(int customerId)
        {
            var accounts = await _accountsRepository.GetAll();
            var customerAccounts = accounts.FindAll(a => a.CustomerID == customerId);
            return customerAccounts;
        }

        public async Task<List<Transactions>> GetLast10Transactions(long accountNumber)
        {
            var transactions = await _transactionsRepository.GetAll();
            var last10Transactions = transactions
                .Where(t => t.SourceAccountNumber == accountNumber || t.DestinationAccountNumber == accountNumber)
                .OrderByDescending(t => t.TransactionDate)
                .Take(10)
                .ToList();
            return last10Transactions;
        }

        public async Task<List<Transactions>> GetLastMonthTransactions(long accountNumber)
        {
            var lastMonth = DateTime.Now.AddMonths(-1);
            var transactions = await _transactionsRepository.GetAll();
            var lastMonthTransactions = transactions
                .Where(t => (t.SourceAccountNumber == accountNumber || t.DestinationAccountNumber == accountNumber) &&
                            t.TransactionDate >= lastMonth)
                .ToList();
            return lastMonthTransactions;
        }

        public async Task<List<Transactions>> GetTransactionsBetweenDates(long accountNumber, DateTime startDate, DateTime endDate)
        {
            var transactions = await _transactionsRepository.GetAll();
            var filteredTransactions = transactions
                .Where(t => (t.SourceAccountNumber == accountNumber || t.DestinationAccountNumber == accountNumber) &&
                            t.TransactionDate >= startDate && t.TransactionDate <= endDate)
                .ToList();
            return filteredTransactions;
        }

        //public async Task<Accounts> OpenNewAccount(AccountOpeningDTO accountOpeningDTO)
        //{
        //    // Create a new account instance
        //    var newAccount = new Accounts
        //    {

        //        Balance = 0,
        //        AccountType = accountOpeningDTO.AccountType,
        //        Status = "Active",
        //        IFSC = accountOpeningDTO.IFSC,
        //        CustomerID = accountOpeningDTO.CustomerID
        //    };


        //    var addedAccount = await _accountsRepository.Add(newAccount);
        //    return addedAccount;
        //}

        public async Task<Accounts> OpenNewAccount(AccountOpeningDTO accountOpeningDTO)
        {
            // Create a new account instance
            var newAccount = new Accounts
            {
                Balance = 0,
                AccountType = accountOpeningDTO.AccountType,
                Status = "Pending", // Set status to Pending
                IFSC = accountOpeningDTO.IFSC,
                CustomerID = accountOpeningDTO.CustomerID
            };

            var addedAccount = await _accountsRepository.Add(newAccount);
            return addedAccount;
        }

    }
}

