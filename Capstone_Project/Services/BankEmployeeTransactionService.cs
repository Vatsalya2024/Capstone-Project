using System;
using Capstone_Project.Controllers;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;

namespace Capstone_Project.Services
{
	public class BankEmployeeTransactionService:IBankEmployeeTransactionService
	{
        private readonly IRepository<int, Transactions> _transactionsRepository;
        private readonly ILogger<CustomerTransactionService> _logger;
        public BankEmployeeTransactionService(IRepository<int, Transactions> transactionsRepository, ILogger<CustomerTransactionService> logger)
		{
            _transactionsRepository = transactionsRepository;
            _logger = logger;
        }

        public async Task<List<Transactions>?> GetAllTransactions()
        {
            try
            {
                _logger.LogInformation("Transactions fetched successfully.");
                return await _transactionsRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all transactions");
                throw new BankTransactionServiceException("Error occurred while retrieving all transactions", ex);
            }
        }

        public async Task<List<Transactions>?> GetTransactionsByAccountNumber(long accountNumber)
        {
            try
            {
                var allTransactions = await _transactionsRepository.GetAll();

                if (allTransactions == null || allTransactions.Count == 0)
                {
                    throw new BankTransactionServiceException($"No transactions found for account number: {accountNumber}");
                }

                var transactions = allTransactions.Where(t => t.SourceAccountNumber == accountNumber).ToList();

                if (transactions == null || transactions.Count == 0)
                {
                    throw new NoAccountsFoundException($"Account not found for account number: {accountNumber}");
                }
                _logger.LogInformation("Transactions fetched successfully.");
                return transactions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving transactions for account number: {accountNumber}");
                throw;
            }
        }





        

        public async Task<double> GetTotalInboundTransactions(long accountNumber)
        {
            try
            {
                var transactions = await _transactionsRepository.GetAll();
                if (transactions == null)
                {
                    throw new BankTransactionServiceException($"No transactions found for account number: {accountNumber}");
                }

                if (!transactions.Any(t => t.SourceAccountNumber == accountNumber))
                {
                    throw new NoAccountsFoundException($"No transactions found for account number: {accountNumber}");
                }

                _logger.LogInformation("Inbound fetched successfully.");
                return transactions.Where(t => t.SourceAccountNumber == accountNumber && t.TransactionType == "Credit").Sum(t => t.Amount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while calculating total inbound transactions for account number: {accountNumber}");
                throw;
            }
        }

        public async Task<double> GetTotalOutboundTransactions(long accountNumber)
        {
            try
            {
                var transactions = await _transactionsRepository.GetAll();
                if (transactions == null)
                {
                    throw new BankTransactionServiceException($"No transactions found for account number: {accountNumber}");
                }

                if (!transactions.Any(t => t.SourceAccountNumber == accountNumber))
                {
                    throw new NoAccountsFoundException($"No transactions found for account number: {accountNumber}");
                }

                _logger.LogInformation("Outbound fetched successfully.");
                return transactions.Where(t => t.SourceAccountNumber == accountNumber && t.TransactionType == "Debit").Sum(t => t.Amount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while calculating total outbound transactions for account number: {accountNumber}");
                throw;
            }
        }



    }
}

