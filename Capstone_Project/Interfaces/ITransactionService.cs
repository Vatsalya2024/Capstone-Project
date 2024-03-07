using System;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Interfaces
{
    public interface ITransactionService
    {
        Task<string> Deposit(int customerId,DepositDTO depositDTO);
        Task<string> Withdraw(int customerId,WithdrawalDTO withdrawalDTO);
        Task<string> Transfer(int customerId,TransferDTO transferDTO);
        Task<List<Transactions>> GetLast10Transactions(long accountNumber);
        Task<List<Transactions>> GetLastMonthTransactions(long accountNumber);
        Task<List<Transactions>> GetTransactionsBetweenDates(long accountNumber, DateTime startDate, DateTime endDate);
        Task<AccountStatementDTO> GetAccountStatement(long accountNumber, DateTime startDate, DateTime endDate);
    }
}

