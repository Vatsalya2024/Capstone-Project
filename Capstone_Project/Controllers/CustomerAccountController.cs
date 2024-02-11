using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Services;
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
        [Route("Open Account")]
        [HttpPost]
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

        [Route("Close Account")]
        [HttpPost]
        public async Task<ActionResult<bool>> CloseAccount(long accountNumber)
        {
            try
            {
                var result = await _accountManagementService.CloseAccount(accountNumber);
                return Ok(result);
            }
            catch (NoAccountsFoundException ex)
            {
                _logger.LogError(ex, $"No account found with number: {accountNumber}");
                return NotFound($"No account found with number: {accountNumber}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error closing account with number: {accountNumber}");
                return StatusCode(500, "Internal server error");
            }
        }
        [Route("GetAccountDetailsByAccountNumber")]
        [HttpGet]
        public async Task<ActionResult<Accounts>> GetAccountDetails(long accountNumber)
        {
            try
            {
                var account = await _accountManagementService.GetAccountDetails(accountNumber);
                
                    return Ok(account);
                
            }
            catch (NoAccountsFoundException ex)
            {
                _logger.LogError(ex, $"No account found with number: {accountNumber}");
                return NotFound($"No account found with number: {accountNumber}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting account details for number: {accountNumber}");
                return StatusCode(500, "Internal server error");
            }
        }
        [Route("GetAccountDetailsByCustomerId")]
        [HttpGet]
        public async Task<ActionResult<List<Accounts>>> GetAllAccountsByCustomerId(int customerId)
        {
            try
            {
                var customerAccounts = await _accountManagementService.GetAllAccountsByCustomerId(customerId);
               return Ok(customerAccounts);
                
                
            }
            catch (NoCustomersFoundException ex)
            {
                _logger.LogError(ex, $"No customer found with ID: {customerId}");
                return NotFound($"No customer found with ID: {customerId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting accounts for customer with ID: {customerId}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

