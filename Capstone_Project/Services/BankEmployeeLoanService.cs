﻿using Capstone_Project.Interfaces;
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


    public async Task<Loans> ReviewLoanApplication(int loanId)
    {
        try
        {
            // Fetch the loan application
            var loan = await _loansRepository.Get(loanId);

            if (loan != null)
            {
                // Log or perform any necessary review actions
                _logger.LogInformation($"Loan application with ID {loanId} reviewed.");
                return loan;
            }
            else
            {
                _logger.LogError($"Loan application with ID {loanId} not found.");
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error reviewing loan application: {ex.Message}");
            return null;
        }
    }

    //public async Task<(bool IsCreditOk, double InboundAmount, double OutboundAmount)> CheckCredit(long accountId)
    //{
    //    try
    //    {
    //        // Get the total inbound amount (credits) for the specified account
    //        var inboundTransactions = await _transactionsRepository.GetAll();
    //        var inboundAmount = inboundTransactions
    //            .Where(t => t.SourceAccountNumber == accountId && t.TransactionType == "Credit")
    //            .Sum(t => t.Amount);

    //        // Get the total outbound amount (debits) for the specified account
    //        var outboundTransactions = await _transactionsRepository.GetAll();
    //        var outboundAmount = outboundTransactions
    //            .Where(t => t.SourceAccountNumber == accountId && t.TransactionType == "Debit")
    //            .Sum(t => t.Amount);

    //        // Check if the inbound amount is greater than or equal to the outbound amount
    //        var isCreditOk = inboundAmount >= outboundAmount;

    //        return (isCreditOk, inboundAmount, outboundAmount);
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError($"Error checking credit for account {accountId}: {ex.Message}");
    //        return (false, 0, 0);
    //    }
    //}

    public async Task<ActionResult<CreditCheckResultDTO>> CheckCredit(long accountId)
    {
        try
        {
            // Get all transactions for the specified account
            var transactions = await _transactionsRepository.GetAll();
            if (transactions != null)
            {
                // Calculate total inbound amount (credits)
                var inboundAmount = transactions
                    .Where(t => t.SourceAccountNumber == accountId && t.TransactionType == "Credit")
                    .Sum(t => t.Amount);

                // Calculate total outbound amount (debits)
                var outboundAmount = transactions
                    .Where(t => t.SourceAccountNumber == accountId && t.TransactionType == "Debit")
                    .Sum(t => t.Amount);

                var creditScore = inboundAmount > outboundAmount ? "Great" : "Bad"; // Determine credit score

                var result = new CreditCheckResultDTO
                {
                    InboundAmount = inboundAmount,
                    OutboundAmount = outboundAmount,
                    CreditScore = creditScore
                };

                return (result);
            }
            else
            {
                return (new CreditCheckResultDTO { InboundAmount = 0, OutboundAmount = 0, CreditScore = "Bad" });
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }





    public async Task<bool> MakeLoanDecision(int loanId, bool approved)
    {
        try
        {
            // Fetch the loan application
            var loan = await _loansRepository.Get(loanId);

            if (loan != null)
            {
                // Update the status based on the decision
                loan.Status = approved ? "Accepted" : "Rejected";

                // Update the loan in the repository
                await _loansRepository.Update(loan);

                _logger.LogInformation($"Loan application with ID {loanId} decision updated to {(approved ? "Accepted" : "Rejected")}.");
                return true;
            }
            else
            {
                _logger.LogError($"Loan application with ID {loanId} not found.");
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error making loan decision: {ex.Message}");
            return false;
        }
    }

    public async Task<Accounts> DisburseLoan(int loanId, long AccId)
    {
        var loan = await _loansRepository.Get(loanId);
        var account = await _accountsRepository.Get(AccId);

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
                Status="Completed",
                SourceAccountNumber = AccId,
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



    //public async Task<bool> DisburseLoan(int loanId)
    //{
    //    try
    //    {
    //        // Fetch the loan application
    //        var loan = await _loansRepository.Get(loanId);

    //        if (loan != null && loan.Status == "Accepted" && loan.Customers != null && loan.Customers.Accounts != null)
    //        {
    //            // Update the loan status to reflect disbursement
    //            loan.Status = "Disbursed";
    //            await _loansRepository.Update(loan);

    //            foreach (var account in loan.Customers.Accounts)
    //            {
    //                var transaction = new Transactions
    //                {
    //                    Amount = loan.LoanAmount,
    //                    TransactionDate = DateTime.Now,
    //                    Description = "Loan Disbursement",
    //                    TransactionType = "Credit",
    //                    SourceAccountNumber = account.AccountNumber // Access AccountNumber from each account
    //                };

    //                await _transactionsRepository.Add(transaction);

    //                // Update the balance in the associated account
    //                var existingAccount = await _accountsRepository.Get(account.AccountNumber);
    //                if (existingAccount != null)
    //                {
    //                    existingAccount.Balance += loan.LoanAmount;
    //                    await _accountsRepository.Update(existingAccount);
    //                }
    //                else
    //                {
    //                    _logger.LogError($"Account not found for customer ID: {loan.CustomerID}");
    //                    return false;
    //                }
    //            }

    //            _logger.LogInformation($"Loan with ID {loanId} disbursed successfully.");
    //            return true;
    //        }
    //        else
    //        {
    //            _logger.LogError($"Loan with ID {loanId} not found, not approved for disbursement, or missing customer or account information.");
    //            return false;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError($"Error disbursing loan: {ex.Message}");
    //        return false;
    //    }
    //}



}