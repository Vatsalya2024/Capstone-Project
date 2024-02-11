using System;
using System.Threading.Tasks;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Repositories;
using Microsoft.Extensions.Logging;
using Capstone_Project.Mappers; 
using Capstone_Project.Interfaces;
using Capstone_Project.Exceptions;

namespace Capstone_Project.Services
{
    public class CustomerTransactionService : ITransactionService
    {
        private readonly ILogger<CustomerTransactionService> _logger;
        private readonly IRepository<int, Transactions> _transactionsRepository;
        private readonly IRepository<long, Accounts> _accountsRepository;
        private readonly TransactionMapper _transactionMapper;

        public CustomerTransactionService(
            ILogger<CustomerTransactionService> logger,
            IRepository<int, Transactions> transactionsRepository,
            IRepository<long, Accounts> accountsRepository,
            TransactionMapper transactionMapper)
        {
            _logger = logger;
            _transactionsRepository = transactionsRepository;
            _accountsRepository = accountsRepository;
            _transactionMapper = transactionMapper; 
        }

        public async Task<string> Deposit(DepositDTO depositDTO)
        {
            try
            {
                var account = await _accountsRepository.Get(depositDTO.AccountNumber);
                if (account != null && account.Status == "Active")
                {
                    if (depositDTO.Amount <= 0)
                    {
                        var errorMessage = "Deposit amount should be greater than zero.";
                        _logger.LogError(errorMessage);
                        return errorMessage;
                    }

                    var transaction = _transactionMapper.MapDepositDtoToTransaction(depositDTO);
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

        public async Task<string> Withdraw(WithdrawalDTO withdrawalDTO)
        {
            try
            {
                var account = await _accountsRepository.Get(withdrawalDTO.AccountNumber);
                if (account != null && account.Status == "Active")
                {
                    if (withdrawalDTO.Amount <= 0)
                    {
                        var errorMessage = "Withdrawal amount should be greater than zero.";
                        _logger.LogError(errorMessage);
                        return errorMessage;
                    }

                    if (account.Balance < withdrawalDTO.Amount)
                        throw new NotSufficientBalanceException();

                    var transaction = _transactionMapper.MapWithdrawalDtoToTransaction(withdrawalDTO);
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

        public async Task<string> Transfer(TransferDTO transferDTO)
        {
            try
            {
                var sourceAccount = await _accountsRepository.Get(transferDTO.SourceAccountNumber);
                if (sourceAccount != null && sourceAccount.Status == "Active")
                {
                    if (transferDTO.Amount <= 0)
                    {
                        var errorMessage = "Transfer amount should be greater than zero.";
                        _logger.LogError(errorMessage);
                        return errorMessage;
                    }

                    if (sourceAccount.Balance < transferDTO.Amount)
                        throw new NotSufficientBalanceException();

                    var sourceTransaction = _transactionMapper.MapTransferDtoToSourceTransaction(transferDTO);
                    var destinationTransaction = _transactionMapper.MapTransferDtoToDestinationTransaction(transferDTO);

                    await _transactionsRepository.Add(sourceTransaction);
                    await _transactionsRepository.Add(destinationTransaction);

                    // Update source account balance
                    sourceAccount.Balance -= transferDTO.Amount;
                    await _accountsRepository.Update(sourceAccount);

                    // Update destination account balance
                    var destinationAccount = await _accountsRepository.Get(transferDTO.DestinationAccountNumber);
                    if (destinationAccount != null && destinationAccount.Status == "Active")
                    {
                        destinationAccount.Balance += transferDTO.Amount;
                        await _accountsRepository.Update(destinationAccount);
                    }

                    var successMessage = "Transfer successful.";
                    _logger.LogInformation(successMessage);
                    return successMessage;
                }
                else
                {
                    var errorMessage = "Source account not found or inactive";
                    _logger.LogError(errorMessage);
                    return errorMessage;
                }
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
                var last10Transactions = transactions
                    .Where(t => t.SourceAccountNumber == accountNumber || t.DestinationAccountNumber == accountNumber)
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
                throw; // Re-throw the exception for handling in the controller
            }
        }

        public async Task<List<Transactions>> GetLastMonthTransactions(long accountNumber)
        {
            try
            {
                var lastMonth = DateTime.Now.AddMonths(-1);
                var transactions = await _transactionsRepository.GetAll();
                var lastMonthTransactions = transactions
                    .Where(t => (t.SourceAccountNumber == accountNumber || t.DestinationAccountNumber == accountNumber) &&
                                t.TransactionDate >= lastMonth)
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
                throw; // Re-throw the exception for handling in the controller
            }
        }

        public async Task<List<Transactions>> GetTransactionsBetweenDates(long accountNumber, DateTime startDate, DateTime endDate)
        {
            try
            {
                var transactions = await _transactionsRepository.GetAll();
                var filteredTransactions = transactions
                    .Where(t => (t.SourceAccountNumber == accountNumber || t.DestinationAccountNumber == accountNumber) &&
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
                throw; // Re-throw the exception for handling in the controller
            }
        }
    }
}
