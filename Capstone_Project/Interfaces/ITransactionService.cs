using System;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Interfaces
{
    public interface ITransactionService
    {
        Task<bool> Deposit(DepositDTO depositDTO);
        Task<bool> Withdraw(WithdrawalDTO withdrawalDTO);
        Task<bool> Transfer(TransferDTO transferDTO);
    }
}

