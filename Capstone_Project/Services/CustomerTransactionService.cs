using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone_Project.Exceptions;
using Capstone_Project.Interfaces;
using Capstone_Project.Mappers;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Repositories;
using Microsoft.Extensions.Logging;

namespace Capstone_Project.Services
{
    public class CustomerTransactionService : ITransactionService
    {
        private readonly ILogger<CustomerTransactionService> _logger;
        private readonly IRepository<int, Transactions> _transactionsRepository;
        private readonly IRepository<long, Accounts> _accountsRepository;

        public CustomerTransactionService(
            ILogger<CustomerTransactionService> logger,
            IRepository<int, Transactions> transactionsRepository,
            IRepository<long, Accounts> accountsRepository)
        {
            _logger = logger;
            _transactionsRepository = transactionsRepository;
            _accountsRepository = accountsRepository;
        }

        public async Task<string> Deposit(int customerId,DepositDTO depositDTO)
        {
            try
            {
                var account = await _accountsRepository.Get(depositDTO.AccountNumber);
                if (account != null && account.CustomerID==customerId && account.Status == "Active")
                {
                    if (depositDTO.Amount <= 0)
                    {
                        var errorMessage = "Deposit amount should be greater than zero.";
                        _logger.LogError(errorMessage);
                        return errorMessage;
                    }

                    var transactionMapper = new TransactionMapper(depositDTO);
                    var transaction = transactionMapper.GetTransaction();

                    await _transactionsRepository.Add(transaction);

                    account.Balance += depositDTO.Amount;
                    await _accountsRepository.Update(account);

                    var successMessage = "Deposit successful.";
                    _logger.LogInformation(successMessage);
                    return successMessage;
                }
                else
                {
                    var errorMessage = "Account not found or inactive";
                    _logger.LogError(errorMessage);
                    return errorMessage;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing deposit.");
                throw;
            }
        }

        public async Task<string> Withdraw(int customerId,WithdrawalDTO withdrawalDTO)
        {
            try
            {
                var account = await _accountsRepository.Get(withdrawalDTO.AccountNumber);
                if (account != null&& account.CustomerID==customerId && account.Status == "Active")
                {
                    if (withdrawalDTO.Amount <= 0)
                    {
                        var errorMessage = "Withdrawal amount should be greater than zero.";
                        _logger.LogError(errorMessage);
                        return errorMessage;
                    }

                    if (account.Balance < withdrawalDTO.Amount)
                        throw new NotSufficientBalanceException();

                    var transactionMapper = new TransactionMapper(withdrawalDTO);
                    var transaction = transactionMapper.GetTransaction();

                    await _transactionsRepository.Add(transaction);

                    account.Balance -= withdrawalDTO.Amount;
                    await _accountsRepository.Update(account);

                    var successMessage = "Withdrawal successful.";
                    _logger.LogInformation(successMessage);
                    return successMessage;
                }
                else
                {
                    var errorMessage = "Account not found or inactive";
                    _logger.LogError(errorMessage);
                    return errorMessage;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing withdrawal.");
                throw;
            }
        }


        public async Task<string> Transfer(int customerId, TransferDTO transferDTO)
        {
            try
            {
                var sourceAccount = await _accountsRepository.Get(transferDTO.SourceAccountNumber);
                var destinationAccount = await _accountsRepository.Get(transferDTO.DestinationAccountNumber);

                if (sourceAccount == null || sourceAccount.CustomerID != customerId || sourceAccount.Status != "Active")
                {
                    var errorMessage = "Source account not found or inactive";
                    _logger.LogError(errorMessage);
                    return errorMessage;
                }

                if (destinationAccount == null || destinationAccount.Status != "Active")
                {
                    var errorMessage = "Destination account not found or inactive";
                    _logger.LogError(errorMessage);
                    return errorMessage;
                }

                if (transferDTO.Amount <= 0)
                {
                    var errorMessage = "Transfer amount should be greater than zero.";
                    _logger.LogError(errorMessage);
                    return errorMessage;
                }

                if (sourceAccount.Balance < transferDTO.Amount)
                    throw new NotSufficientBalanceException();

                
                var sourceTransaction = new TransactionMapper(transferDTO, true).GetTransaction();
                var destinationTransaction = new TransactionMapper(transferDTO, false).GetTransaction();

               
                await _transactionsRepository.Add(sourceTransaction);
                await _transactionsRepository.Add(destinationTransaction);

                
                sourceAccount.Balance -= transferDTO.Amount;
                destinationAccount.Balance += transferDTO.Amount;

                await _accountsRepository.Update(sourceAccount);
                await _accountsRepository.Update(destinationAccount);

                var successMessage = "Transfer successful.";
                _logger.LogInformation(successMessage);
                return successMessage;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing transfer.");
                throw;
            }
        }






        public async Task<List<Transactions>> GetLast10Transactions(long accountNumber)
        {
            try
            {
                var transactions = await _transactionsRepository.GetAll();
                if (transactions == null)
                {
                    throw new NoTransactionsException("No transaction found");
                }
                var last10Transactions = transactions
                    .Where(t => t.SourceAccountNumber == accountNumber)
                    .OrderByDescending(t => t.TransactionDate)
                    .Take(10)
                    .ToList();
                if (last10Transactions.Count == 0)
                {
                    throw new NoTransactionsException("No transactions found for the account.");
                }
                return last10Transactions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving last 10 transactions.");
                throw; 
            }
        }

      

        public async Task<List<Transactions>> GetLastMonthTransactions(long accountNumber)
        {
            try
            {
                var currentDate = DateTime.Now;
                var firstDayOfLastMonth = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(-1);
                var lastDayOfLastMonth = new DateTime(currentDate.Year, currentDate.Month, 1).AddDays(-1);

                var transactions = await _transactionsRepository.GetAll();
                if (transactions == null)
                {
                    throw new NoTransactionsException("No transaction found");
                }
                var lastMonthTransactions = transactions
                    .Where(t => (t.SourceAccountNumber == accountNumber) &&
                                t.TransactionDate >= firstDayOfLastMonth &&
                                t.TransactionDate <= lastDayOfLastMonth)
                    .ToList();

                if (lastMonthTransactions.Count == 0)
                {
                    throw new NoTransactionsException("No transactions found for the account in the last month.");
                }

                return lastMonthTransactions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving last month transactions.");
                throw; 
            }
        }


        public async Task<List<Transactions>> GetTransactionsBetweenDates(long accountNumber, DateTime startDate, DateTime endDate)
        {
            try
            {
                var transactions = await _transactionsRepository.GetAll();
                if (transactions == null)
                {
                    throw new NoTransactionsException("No transaction found");
                }
                var filteredTransactions = transactions
                    .Where(t => (t.SourceAccountNumber == accountNumber) &&
                                t.TransactionDate >= startDate && t.TransactionDate <= endDate)
                    .ToList();
                if (filteredTransactions.Count == 0)
                {
                    throw new NoTransactionsException("No transactions found for the account within the specified dates.");
                }
                return filteredTransactions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving transactions between dates.");
                throw; 
            }
        }
        public async Task<AccountStatementDTO> GetAccountStatement(long accountNumber, DateTime startDate, DateTime endDate)
        {
            try
            {
                var transactions = await _transactionsRepository.GetAll();
                if (transactions == null || transactions.Count == 0)
                {
                    throw new NoTransactionsException("No transactions found.");
                }

                var accountTransactions = transactions.Where(t => t.SourceAccountNumber == accountNumber &&
                                                                   t.TransactionDate >= startDate &&
                                                                   t.TransactionDate <= endDate)
                                                      .ToList();

                if (accountTransactions.Count == 0)
                {
                    throw new NoTransactionsException("No transactions found for the specified account within the specified dates.");
                }

                double totalDebit = accountTransactions.Where(t => t.TransactionType == "Debit").Sum(t => t.Amount);
                double totalCredit = accountTransactions.Where(t => t.TransactionType == "Credit").Sum(t => t.Amount);

                string creditScore = totalDebit < totalCredit ? "Good" : "Bad";

                var accountStatementDTO = new AccountStatementDTO
                {
                    CreditScore = creditScore,
                    TotalDebit = totalDebit,
                    TotalCredit = totalCredit
                };

                return accountStatementDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving account statement.");
                throw;
            }
        }

    }
}
