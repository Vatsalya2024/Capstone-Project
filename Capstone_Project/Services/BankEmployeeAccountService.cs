using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Microsoft.Extensions.Logging;

namespace Capstone_Project.Services
{
    public class BankEmployeeAccountService : IBankEmployeeAccountService
    {
        private readonly IRepository<long, Accounts> _accountsRepository;
        private readonly ILogger<BankEmployeeAccountService> _logger;
        private readonly IRepository<int, Customers> _customerRepository;

        public BankEmployeeAccountService(IRepository<long, Accounts> accountsRepository, IRepository<int,Customers> customerRepository,ILogger<BankEmployeeAccountService> logger)
        {
            _accountsRepository = accountsRepository;
            _logger = logger;
            _customerRepository = customerRepository;
        }

        public async Task<Customers> GetCustomers(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching customer with ID {id}...");

                var customer = await _customerRepository.Get(id);
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching customer with ID {id}.");
                throw;
            }
        }
        public async Task<List<Customers>> GetCustomersListasync()
        {
            try
            {
                _logger.LogInformation("Fetching customer list...");

                var customer = await _customerRepository.GetAll();
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the customer list.");
                throw;
            }
        }


        public async Task<bool> ApproveAccountCreation(long accountNumber)
        {
            try
            {
                var account = await _accountsRepository.Get(accountNumber);

                if (account != null && account.Status == "Pending")
                {
                    account.Status = "Active"; 
                    await _accountsRepository.Update(account);
                    _logger.LogInformation($"Account creation approved for account number: {accountNumber}");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"Account creation approval failed for account number: {accountNumber}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error approving account creation for account number {accountNumber}: {ex.Message}");
                throw new AccountApprovalException($"Error approving account creation for account number {accountNumber}: {ex.Message}");
            }
        }

        public async Task<bool> ApproveAccountDeletion(long accountNumber)
        {
            try
            {
                var account = await _accountsRepository.Get(accountNumber);

                if (account != null && account.Status == "PendingDeletion")
                {
                    await _accountsRepository.Delete(accountNumber);
                    _logger.LogInformation($"Account deletion approved for account number: {accountNumber}");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"Account deletion approval failed for account number: {accountNumber}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error approving account deletion for account number {accountNumber}: {ex.Message}");
                throw new AccountApprovalException($"Error approving account deletion for account number {accountNumber}: {ex.Message}");
            }
        }

        public async Task<List<Accounts>> GetPendingAccounts()
        {
            try
            {
                var allAccounts = await _accountsRepository.GetAll();
                
                var pendingAccounts = allAccounts.Where(account => account.Status == "Pending").ToList();
                return pendingAccounts;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching pending accounts: {ex.Message}");
                throw new AccountFetchException($"Error fetching pending accounts: {ex.Message}");
            }
        }

        public async Task<List<Accounts>> GetPendingDeletionAccounts()
        {
            try
            {
                var allAccounts = await _accountsRepository.GetAll();
          
                var pendingDeletionAccounts = allAccounts.Where(account => account.Status == "PendingDeletion").ToList();
                return pendingDeletionAccounts;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching accounts pending deletion: {ex.Message}");
                throw new AccountFetchException($"Error fetching accounts pending deletion: {ex.Message}");
            }
        }
    }

    
}
