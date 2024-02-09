using Microsoft.AspNetCore.Mvc;
using Capstone_Project.Interfaces;
using Capstone_Project.Models.DTOs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Capstone_Project.Models;

namespace Capstone_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerLoanController : ControllerBase
    {
        private readonly ILoanCustomerService _loanCustomerService;
        private readonly ILogger<CustomerLoanController> _logger;

        public CustomerLoanController(ILoanCustomerService loanCustomerService, ILogger<CustomerLoanController> logger)
        {
            _loanCustomerService = loanCustomerService;
            _logger = logger;
        }

        [HttpPost("apply")]
        public async Task<IActionResult> ApplyForLoan([FromBody] LoanApplicationDTO loanApplication)
        {
            try
            {
                var success = await _loanCustomerService.ApplyForLoan(loanApplication);
                if (success)
                {
                    return Ok("Loan application submitted successfully.");
                }
                else
                {
                    return BadRequest("Failed to submit loan application.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error applying for loan: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("availed")]
        public async Task<IActionResult> ViewAvailedLoans()
        {
            try
            {
                var availedLoans = await _loanCustomerService.ViewAvailedLoans();
                return Ok(availedLoans);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error viewing availed loans: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        
    }
}
