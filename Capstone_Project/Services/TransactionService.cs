using System;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IRepository<int, Transactions> _transactionsRepository;
        private readonly IRepository<long, Accounts> _accountsRepository;

        public TransactionService(IRepository<int, Transactions> transactionsRepository, IRepository<long, Accounts> accountsRepository)
        {
            _transactionsRepository = transactionsRepository;
            _accountsRepository = accountsRepository;
        }

        public async Task<bool> Deposit(DepositDTO depositDTO)
        {
            if (depositDTO.Amount <= 0)
                throw new ArgumentException("Deposit amount should be greater than zero.");

            var transaction = new Transactions
            {
                Amount = depositDTO.Amount,
                Description = "Deposit",
                TransactionType = "Credit",
                Status = "Completed",
                SourceAccountNumber = depositDTO.AccountNumber,
            };

            await _transactionsRepository.Add(transaction);

            // Update account balance
            var account = await _accountsRepository.Get(depositDTO.AccountNumber);
            if (account != null)
            {
                account.Balance += depositDTO.Amount;
                await _accountsRepository.Update(account);
            }

            return true;
        }

        public async Task<bool> Withdraw(WithdrawalDTO withdrawalDTO)
        {
            if (withdrawalDTO.Amount <= 0)
                throw new ArgumentException("Withdrawal amount should be greater than zero.");

            var account = await _accountsRepository.Get(withdrawalDTO.AccountNumber);
            if (account != null)
            {
                if (account.Balance < withdrawalDTO.Amount)
                    throw new NotSufficientBalanceException();

                var transaction = new Transactions
                {
                    Amount = withdrawalDTO.Amount,
                    Description = "Withdrawal",
                    TransactionType = "Debit",
                    Status = "Completed",
                    SourceAccountNumber = withdrawalDTO.AccountNumber,
                };

                await _transactionsRepository.Add(transaction);

                // Update account balance
                account.Balance -= withdrawalDTO.Amount;
                await _accountsRepository.Update(account);

                return true;
            }
            else
            {
                return false; // Account not found
            }
        }

        public async Task<bool> Transfer(TransferDTO transferDTO)
        {
            if (transferDTO.Amount <= 0)
                throw new ArgumentException("Transfer amount should be greater than zero.");

            var sourceAccount = await _accountsRepository.Get(transferDTO.SourceAccountNumber);
            if (sourceAccount != null)
            {
                if (sourceAccount.Balance < transferDTO.Amount)
                    throw new NotSufficientBalanceException();

                var sourceTransaction = new Transactions
                {
                    Amount = transferDTO.Amount,
                    Description = "Transfer to " + transferDTO.DestinationAccountNumber,
                    TransactionType = "Debit",
                    Status = "Completed",
                    SourceAccountNumber = transferDTO.SourceAccountNumber,
                    DestinationAccountNumber = transferDTO.DestinationAccountNumber
                };

                var destinationTransaction = new Transactions
                {
                    Amount = transferDTO.Amount,
                    Description = "Transfer from " + transferDTO.SourceAccountNumber,
                    TransactionType = "Credit",
                    Status = "Completed",
                    SourceAccountNumber = transferDTO.SourceAccountNumber,
                    DestinationAccountNumber = transferDTO.DestinationAccountNumber
                };

                await _transactionsRepository.Add(sourceTransaction);
                await _transactionsRepository.Add(destinationTransaction);

                // Update source account balance
                sourceAccount.Balance -= transferDTO.Amount;
                await _accountsRepository.Update(sourceAccount);

                // Update destination account balance
                var destinationAccount = await _accountsRepository.Get(transferDTO.DestinationAccountNumber);
                if (destinationAccount != null)
                {
                    destinationAccount.Balance += transferDTO.Amount;
                    await _accountsRepository.Update(destinationAccount);
                }

                return true;
            }
            else
            {
                return false; // Source account not found
            }
        }
    }

}