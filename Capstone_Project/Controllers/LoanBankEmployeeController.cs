using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Capstone_Project.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class LoanBanKEmployeeController : ControllerBase
{
    private readonly IBankEmployeeLoanService _bankEmployeeLoanService;

    public LoanBanKEmployeeController(IBankEmployeeLoanService bankEmployeeLoanService)
    {
        _bankEmployeeLoanService = bankEmployeeLoanService;
    }

    [HttpPost("review")]
    public async Task<IActionResult> ReviewLoanApplication(int loanId)
    {
        try
        {
            var result = await _bankEmployeeLoanService.ReviewLoanApplication(loanId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error reviewing loan application: {ex.Message}");
        }
    }

    [HttpPost("checkcredit")]
    public async Task<IActionResult> CheckCredit(long accountId)
    {
        try
        {
            var result = await _bankEmployeeLoanService.CheckCredit(accountId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error checking credit: {ex.Message}");
        }
    }


    [HttpPost("decision")]
    public async Task<IActionResult> MakeLoanDecision(int loanId, bool approved)
    {
        try
        {
            var result = await _bankEmployeeLoanService.MakeLoanDecision(loanId, approved);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error making loan decision: {ex.Message}");
        }
    }

    [HttpPost("disburse")]
    public async Task<IActionResult> DisburseLoan(int loanId, long AccId)
    {
        try
        {
            var result = await _bankEmployeeLoanService.DisburseLoan(loanId, AccId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error disbursing loan: {ex.Message}");
        }
    }
}
