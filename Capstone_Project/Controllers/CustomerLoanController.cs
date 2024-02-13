using Microsoft.AspNetCore.Mvc;
using Capstone_Project.Interfaces;
using Capstone_Project.Models.DTOs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Capstone_Project.Models;
using Capstone_Project.Services;

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

      
        [Route("ApplyForLoan")]
        [HttpPost]
        public async Task<IActionResult> ApplyForLoan(LoanApplicationDTO loanApplication)
        {
            try
            {
                await _loanCustomerService.ApplyForLoan(loanApplication);
                return Ok("Loan application submitted successfully.");
            }
            catch (NoCustomersFoundException ex)
            {
                _logger.LogError(ex, $"Error applying for loan: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying for loan.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [Route("AvailedLoans")]
        [HttpGet]
        public async Task<IActionResult> ViewAvailedLoans(int customerId)
        {
            try
            {
                var availedLoans = await _loanCustomerService.ViewAvailedLoans(customerId);
                return Ok(availedLoans);
            }
            catch (NoCustomersFoundException ex)
            {
                _logger.LogError(ex, $"Error viewing availed loans: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (NoLoansFoundException ex)
            {
                _logger.LogError(ex, $"Error viewing availed loans: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error viewing availed loans.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
