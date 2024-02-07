using System;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Interfaces
{
    public interface IAccountManagementService
    {
        Task<Accounts> OpenNewAccount(AccountOpeningDTO accountOpeningDTO);
        Task<bool> CloseAccount(long accountNumber);
        Task<List<Accounts>> GetAllAccountsByCustomerId(int customerId);
        Task<Accounts> GetAccountDetails(long accountNumber);
        Task<List<Transactions>> GetLast10Transactions(long accountNumber);
        Task<List<Transactions>> GetLastMonthTransactions(long accountNumber);
        Task<List<Transactions>> GetTransactionsBetweenDates(long accountNumber, DateTime startDate, DateTime endDate);
    }
}

