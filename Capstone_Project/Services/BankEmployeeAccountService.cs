using System;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Repositories;
using Microsoft.Extensions.Logging; // Add this namespace

namespace Capstone_Project.Services
{
    public class BankEmployeeAccountService : IBankEmployeeAccountService
    {
        private readonly IRepository<long, Accounts> _accountsRepository;
        private readonly ILogger<BankEmployeeAccountService> _logger; // Logger instance

        public BankEmployeeAccountService(IRepository<long, Accounts> accountsRepository, ILogger<BankEmployeeAccountService> logger)
        {
            _accountsRepository = accountsRepository;
            _logger = logger; // Initialize logger
        }

        public async Task<bool> ApproveAccountCreation(long accountNumber)
        {
            try
            {
                var account = await _accountsRepository.Get(accountNumber);

                if (account != null && account.Status == "Pending")
                {
                    account.Status = "Active"; // Approve the account creation
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
                throw;
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
                throw;
            }
        }

        public async Task<List<Accounts>> GetPendingAccounts()
        {
            try
            {
                var allAccounts = await _accountsRepository.GetAll();
                // Filter accounts with "Pending" status
                var pendingAccounts = allAccounts.Where(account => account.Status == "Pending").ToList();
                return pendingAccounts;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching pending accounts: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Accounts>> GetPendingDeletionAccounts()
        {
            try
            {
                var allAccounts = await _accountsRepository.GetAll();
                // Filter accounts with "PendingDeletion" status
                var pendingDeletionAccounts = allAccounts.Where(account => account.Status == "PendingDeletion").ToList();
                return pendingDeletionAccounts;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching accounts pending deletion: {ex.Message}");
                throw;
            }
        }
    }
}




//public async Task<bool> ApproveAccountCreation(long accountNumber)
//{
//    var account = await _accountsRepository.Get(accountNumber);

//    if (account != null && account.Status == "Pending")
//    {
//        account.Status = "Active"; // Approve the account creation
//        await _accountsRepository.Update(account);
//        return true;
//    }
//    return false;
//}

//public async Task<bool> ApproveAccountDeletion(long accountNumber)
//{
//    var account = await _accountsRepository.Get(accountNumber);

//    if (account != null && account.Status == "PendingDeletion")
//    {
//        await _accountsRepository.Delete(accountNumber);
//        return true;
//    }

//    return false;
//}
