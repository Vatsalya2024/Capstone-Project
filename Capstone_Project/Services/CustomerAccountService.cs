﻿using System;
using Capstone_Project.Controllers;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Services
{
    public class CustomerAccountService : IAccountManagementService
    {
        private readonly IRepository<long, Accounts> _accountsRepository;
        private readonly IRepository<int, Transactions> _transactionsRepository;
        private readonly ILogger<CustomerAccountService> _logger;

        public CustomerAccountService(IRepository<long, Accounts> accountsRepository, IRepository<int, Transactions> transactionsRepository,ILogger<CustomerAccountService>  logger)
        {
            _accountsRepository = accountsRepository;
            _transactionsRepository = transactionsRepository;
            _logger = logger;
        }


        public async Task<string> CloseAccount(long accountNumber)
        {
            try
            {
                var account = await _accountsRepository.Get(accountNumber);

                if (account != null)
                {
                    account.Status = "PendingDeletion";
                    await _accountsRepository.Update(account);
                    return $"Account with number {accountNumber} is scheduled for deletion.";
                }
                else
                {
                    throw new NoAccountsFoundException($"No account found with number: {accountNumber}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error closing account with number: {accountNumber}");
                throw;
            }
        }


        public async Task<Accounts> GetAccountDetails(long accountNumber)
        {
            try
            {
                var account = await _accountsRepository.Get(accountNumber);

                if (account != null)
                {
                    return account;
                }
                else
                {
                    throw new NoAccountsFoundException($"No account found with number: {accountNumber}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting account details for number: {accountNumber}");
                throw;
            }
        }

        public async Task<List<Accounts>> GetAllAccountsByCustomerId(int customerId)
        {
            try
            {
                var accounts = await _accountsRepository.GetAll();
                var customerAccounts = accounts.FindAll(a => a.CustomerID == customerId);

                if (customerAccounts.Count > 0)
                {
                    return customerAccounts;
                }
                else
                {
                    throw new NoCustomersFoundException($"No customer found with ID: {customerId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting accounts for customer with ID: {customerId}");
                throw;
            }
        }


    

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

