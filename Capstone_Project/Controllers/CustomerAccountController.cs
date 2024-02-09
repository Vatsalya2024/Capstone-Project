using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Capstone_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerAccountController : ControllerBase
    {
        private readonly IAccountManagementService _accountManagementService;
        private readonly ILogger<CustomerAccountController> _logger;

        public CustomerAccountController(IAccountManagementService accountManagementService, ILogger<CustomerAccountController> logger)
        {
            _accountManagementService = accountManagementService;
            _logger = logger;
        }

        [HttpPost("OpenAccount")]
        public async Task<ActionResult<Accounts>> OpenAccount(AccountOpeningDTO accountOpeningDTO)
        {
            try
            {
                var newAccount = await _accountManagementService.OpenNewAccount(accountOpeningDTO);
                return Ok(newAccount);
            }
            catch (NoAccountsFoundException nafe)
            {
                _logger.LogError(nafe, "Error occurred while opening account");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("CloseAccount/{accountNumber}")]
        public async Task<ActionResult> CloseAccount(long accountNumber)
        {
            try
            {
                var result = await _accountManagementService.CloseAccount(accountNumber);
                if (result)
                {
                    return Ok("Account Closing Request Submitted.");
                }
                else
                {
                    return NotFound("Account not found.");
                }
            }
            catch (NoAccountsFoundException nafe)
            {
                _logger.LogError(nafe, "Error occurred while closing account");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("GetAccountDetails/{accountNumber}")]
        public async Task<ActionResult<Accounts>> GetAccountDetails(long accountNumber)
        {
            try
            {
                var account = await _accountManagementService.GetAccountDetails(accountNumber);
                if (account != null)
                {
                    return Ok(account);
                }
                else
                {
                    return NotFound("Account not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching account details");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetAllAccountsByCustomerId/{customerId}")]
        public async Task<ActionResult<List<Accounts>>> GetAllAccountsByCustomerId(int customerId)
        {
            try
            {
                var accounts = await _accountManagementService.GetAllAccountsByCustomerId(customerId);
                if (accounts != null && accounts.Count > 0)
                {
                    return Ok(accounts);
                }
                else
                {
                    return NotFound("No accounts found for the customer.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching accounts for the customer");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("GetLast10Transactions/{accountNumber}")]
        public async Task<ActionResult<List<Transactions>>> GetLast10Transactions(long accountNumber)
        {
            try
            {
                var transactions = await _accountManagementService.GetLast10Transactions(accountNumber);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching last 10 transactions");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetLastMonthTransactions/{accountNumber}")]
        public async Task<ActionResult<List<Transactions>>> GetLastMonthTransactions(long accountNumber)
        {
            try
            {
                var transactions = await _accountManagementService.GetLastMonthTransactions(accountNumber);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching last month transactions");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetTransactionsBetweenDates/{accountNumber}")]
        public async Task<ActionResult<List<Transactions>>> GetTransactionsBetweenDates(long accountNumber, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var transactions = await _accountManagementService.GetTransactionsBetweenDates(accountNumber, startDate, endDate);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching transactions between dates");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

