using Capstone_Project.Controllers;
using Capstone_Project.Exceptions;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

public class BankEmployeeLoanService : IBankEmployeeLoanService
{
    private readonly IRepository<int, Loans> _loansRepository;
    private readonly IRepository<long, Accounts> _accountsRepository;
    private readonly IRepository<int, Transactions> _transactionsRepository;
    private readonly ILogger<BankEmployeeLoanService> _logger;

    public BankEmployeeLoanService(
        IRepository<int, Loans> loansRepository,
        IRepository<long, Accounts> accountsRepository,
        IRepository<int, Transactions> transactionsRepository,
        ILogger<BankEmployeeLoanService> logger)
    {
        _loansRepository = loansRepository;
        _accountsRepository = accountsRepository;
        _transactionsRepository = transactionsRepository;
        _logger = logger;
    }

    public async Task<List<Loans>> GetAllLoans()
    {
        try
        {
            var loans = await _loansRepository.GetAll();
            if (loans == null || loans.Count == 0)
            {
                throw new NoLoansFoundException("No loans found.");
            }
            return loans;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving all loans: {ex.Message}");
            throw;
        }
    }


    public async Task<Loans> ReviewLoanApplication(int loanId)
    {
        try
        {
           
            var loan = await _loansRepository.Get(loanId);

            if (loan != null)
            {
                
                _logger.LogInformation($"Loan application with ID {loanId} reviewed.");
                return loan;
            }
            else
            {
                _logger.LogError($"Loan application with ID {loanId} not found.");
                throw new NoLoansFoundException($"Loan application with ID {loanId} not found.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error reviewing loan application: {ex.Message}");
            throw;
        }
    }



    public async Task<ActionResult<CreditCheckResultDTO>> CheckCredit(long accountId)
    {
        try
        {
            
            var transactions = await _transactionsRepository.GetAll();

            if (transactions == null)
            {
                throw new NoTransactionsException($"No transactions found for account ID: {accountId}");
            }

            if (!transactions.Any(t => t.SourceAccountNumber == accountId))
            {
                throw new NoAccountsFoundException($"No account found with ID: {accountId}");
            }

            
            var inboundAmount = transactions
                .Where(t => t.SourceAccountNumber == accountId && t.TransactionType == "Credit")
                .Sum(t => t.Amount);

            
            var outboundAmount = transactions
                .Where(t => t.SourceAccountNumber == accountId && t.TransactionType == "Debit")
                .Sum(t => t.Amount);

            var creditScore = inboundAmount > outboundAmount ? "Good" : "Bad"; 

            var result = new CreditCheckResultDTO
            {
                InboundAmount = inboundAmount,
                OutboundAmount = outboundAmount,
                CreditScore = creditScore
            };

            return (result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error checking credit for account ID: {accountId}");
            throw;
        }
    }


    public async Task<string> MakeLoanDecision(int loanId, bool approved)
    {
        try
        {
           
            var loan = await _loansRepository.Get(loanId);

            if (loan != null)
            {
               
                loan.Status = approved ? "Accepted" : "Rejected";

               
                await _loansRepository.Update(loan);

                _logger.LogInformation($"Loan application with ID {loanId} decision updated to {(approved ? "Accepted" : "Rejected")}.");
                return $"Loan application with ID {loanId} decision updated to {(approved ? "Accepted" : "Rejected")}.";
            }
            else
            {
                _logger.LogError($"Loan application with ID {loanId} not found.");
                throw new NoLoansFoundException($"Loan application with ID {loanId} not found.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error making loan decision: {ex.Message}");
            return $"Error making loan decision: {ex.Message}";
        }
    }




   
    
    public async Task<Accounts> DisburseLoan(int loanId, long accountId)
    {
        try
        {
            var loan = await _loansRepository.Get(loanId);
            var account = await _accountsRepository.Get(accountId);

            if (loan == null)
            {
                throw new NoLoansFoundException($"Loan with ID {loanId} not found.");
            }

            if (account == null)
            {
                throw new NoAccountsFoundException($"Account with ID {accountId} not found.");
            }

            if (loan.Status == "Accepted")
            {
                account.Balance += loan.LoanAmount;
                account = await _accountsRepository.Update(account);

                // Update the status of the loan to "Disbursed"
                loan.Status = "Disbursed";
                await _loansRepository.Update(loan);

                // Create a new transaction record for the disbursement
                var transaction = new Transactions
                {
                    Amount = loan.LoanAmount,
                    TransactionDate = DateTime.Now,
                    Description = "Loan Disbursement",
                    TransactionType = "Credit",
                    Status = "Completed",
                    SourceAccountNumber = accountId,
                };
                await _transactionsRepository.Add(transaction);

                _logger.LogInformation($"Loan with ID {loanId} disbursed successfully.");
            }
            else
            {
                _logger.LogError($"Loan with ID {loanId} is not in an accepted state.");
            }

            return account;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error disbursing loan with ID {loanId}");
            throw;
        }
    }





}
