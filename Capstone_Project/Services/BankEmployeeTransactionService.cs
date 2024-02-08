using System;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;

namespace Capstone_Project.Services
{
	public class BankEmployeeTransactionService:IBankEmployeeTransactionService
	{
        private readonly IRepository<int, Transactions> _transactionsRepository;
        private readonly ILogger<TransactionService> _logger;
        public BankEmployeeTransactionService(IRepository<int, Transactions> transactionsRepository, ILogger<TransactionService> logger)
		{
            _transactionsRepository = transactionsRepository;
            _logger = logger;
        }

        public async Task<List<Transactions>?> GetAllTransactions()
        {
            try
            {
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
                if (allTransactions == null)
                {
                    return null;
                }
                return allTransactions.Where(t => t.SourceAccountNumber == accountNumber || t.DestinationAccountNumber == accountNumber).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving transactions for account number: {accountNumber}");
                throw new BankTransactionServiceException($"Error occurred while retrieving transactions for account number: {accountNumber}", ex);
            }
        }

        //public async Task<double> GetTotalInboundTransactions(long accountNumber)
        //{
        //    try
        //    {
        //        var transactions = await GetTransactionsByAccountNumber(accountNumber);
        //        if (transactions == null)
        //        {
        //            return 0;
        //        }
        //        return transactions.Where(t => t.DestinationAccountNumber == accountNumber).Sum(t => t.Amount);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Error occurred while calculating total inbound transactions for account number: {accountNumber}");
        //        throw new BankTransactionServiceException($"Error occurred while calculating total inbound transactions for account number: {accountNumber}", ex);
        //    }
        //}

        //public async Task<double> GetTotalOutboundTransactions(long accountNumber)
        //{
        //    try
        //    {
        //        var transactions = await GetTransactionsByAccountNumber(accountNumber);
        //        if (transactions == null)
        //        {
        //            return 0;
        //        }
        //        return transactions.Where(t => t.SourceAccountNumber == accountNumber).Sum(t => t.Amount);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Error occurred while calculating total outbound transactions for account number: {accountNumber}");
        //        throw new BankTransactionServiceException($"Error occurred while calculating total outbound transactions for account number: {accountNumber}", ex);
        //    }
        //}

        public async Task<double> GetTotalInboundTransactions(long accountNumber)
        {
            try
            {
                var transactions = await _transactionsRepository.GetAll(); 
                if (transactions == null)
                {
                    return 0;
                }
                // Filter transactions by type Credit and sum their amounts
                return transactions.Where(t => t.SourceAccountNumber == accountNumber && t.TransactionType == "Credit").Sum(t => t.Amount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while calculating total inbound transactions for account number: {accountNumber}");
                throw new BankTransactionServiceException($"Error occurred while calculating total inbound transactions for account number: {accountNumber}", ex);
            }
        }

        public async Task<double> GetTotalOutboundTransactions(long accountNumber)
        {
            try
            {
                var transactions = await _transactionsRepository.GetAll(); ;
                if (transactions == null)
                {
                    return 0;
                }
                // Filter transactions by type Debit and sum their amounts
                return transactions.Where(t => t.SourceAccountNumber == accountNumber && t.TransactionType == "Debit").Sum(t => t.Amount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while calculating total outbound transactions for account number: {accountNumber}");
                throw new BankTransactionServiceException($"Error occurred while calculating total outbound transactions for account number: {accountNumber}", ex);
            }
        }




    }
}

