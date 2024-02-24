using System;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Interfaces
{
    public interface IAccountManagementService
    {
        Task<Accounts> OpenNewAccount(AccountOpeningDTO accountOpeningDTO);
        Task<string> CloseAccount(long accountNumber);
        Task<List<Accounts>> GetAllAccountsByCustomerId(int customerId);
        Task<Accounts> GetAccountDetails(long accountNumber, int customerId);
        //    Task<List<Transactions>> GetLast10Transactions(long accountNumber);
        //    Task<List<Transactions>> GetLastMonthTransactions(long accountNumber);
        //    Task<List<Transactions>> GetTransactionsBetweenDates(long accountNumber, DateTime startDate, DateTime endDate);
        //}
    }
}
