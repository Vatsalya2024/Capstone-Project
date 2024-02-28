using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Controllers;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Exceptions;

[Route("api/[controller]")]
[ApiController]
public class BankEmployeeLoanController : ControllerBase
{
    private readonly IBankEmployeeLoanService _bankEmployeeLoanService;
    private readonly ILogger<BankEmployeeLoanController> _logger;

    public BankEmployeeLoanController(IBankEmployeeLoanService bankEmployeeLoanService,ILogger<BankEmployeeLoanController> logger)
    {
        _bankEmployeeLoanService = bankEmployeeLoanService;
        _logger = logger;
    }
    [Route("GetAllLoans")]
    [HttpGet]
    public async Task<ActionResult<List<Loans>>> GetAllLoans()
    {
        try
        {
            var loans = await _bankEmployeeLoanService.GetAllLoans();
            return loans;
        }
        catch (NoLoansFoundException ex)
        {
            _logger.LogError(ex.Message);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving all loans: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("ReviewLoanApplication/{loanId}")]
    public async Task<ActionResult<Loans>> ReviewLoanApplication(int loanId)
    {
        try
        {
            var loan = await _bankEmployeeLoanService.ReviewLoanApplication(loanId);
            
                return Ok(loan);
      
            
        }
        catch (NoLoansFoundException ex)
        {
            _logger.LogError($"No loan found: {ex.Message}");
            return NotFound($"Loan with ID {loanId} not found.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error reviewing loan application: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }


   
    [HttpGet("check-credit/{accountId}")]
    public async Task<ActionResult<CreditCheckResultDTO>> CheckCredit(long accountId)
    {
        try
        {
            var creditCheckResult = await _bankEmployeeLoanService.CheckCredit(accountId);
            return Ok(creditCheckResult);
        }
        catch (NoAccountsFoundException ex)
        {
            _logger.LogError(ex.Message);
            return NotFound(ex.Message);
        }
        catch (NoTransactionsException ex)
        {
            _logger.LogError(ex.Message);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error checking credit for account ID: {accountId}");
            return StatusCode(500, "Internal server error");
        }
    }


    
    [HttpPost("MakeLoanDecision/{loanId}")]
    public async Task<ActionResult<string>> MakeLoanDecision(int loanId, bool approved)
    {
        try
        {
            var decisionMessage = await _bankEmployeeLoanService.MakeLoanDecision(loanId, approved);
            return Ok(decisionMessage);
        }
        catch (NoLoansFoundException ex)
        {
            _logger.LogError($"{ex.Message}");
            return NotFound($"Loan with ID {loanId} not found.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error making loan decision: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }


    [HttpPost("disburse-loan/{loanId}/{accountId}")]
    public async Task<ActionResult<Accounts>> DisburseLoan(int loanId, long accountId)
    {
        try
        {
            var account = await _bankEmployeeLoanService.DisburseLoan(loanId, accountId);
            return Ok(account);
        }
        catch (NoLoansFoundException ex)
        {
            _logger.LogError(ex.Message);
            return NotFound(ex.Message);
        }
        catch (NoAccountsFoundException ex)
        {
            _logger.LogError(ex.Message);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error disbursing loan with ID {loanId}");
            return StatusCode(500, "Internal server error");
        }
    }
}
