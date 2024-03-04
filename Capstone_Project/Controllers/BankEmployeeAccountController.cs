using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Capstone_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankEmployeeAccountController : ControllerBase
    {
        private readonly IBankEmployeeAccountService _bankEmployeeAccountService;
        private readonly ILogger<BankEmployeeAccountController> _logger;

        public BankEmployeeAccountController(IBankEmployeeAccountService bankEmployeeAccountService, ILogger<BankEmployeeAccountController> logger)
        {
            _bankEmployeeAccountService = bankEmployeeAccountService;
            _logger = logger;
        }
        [Authorize(Roles = "BankEmployee")]
        [Route(("GetAllCustomer"))]
        [HttpGet]
        public async Task<ActionResult<List<Customers>>> GetCustomers()
        {
            try
            {
                var customers = await _bankEmployeeAccountService.GetCustomersListasync();
                _logger.LogInformation("Retrieved Employees successfully.");
                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }
        [Authorize(Roles = "BankEmployee")]
        [Route("GetCustomerById")]
        [HttpGet]
        public async Task<ActionResult<Customers>> GetCustomerByID(int id)
        {
            try
            {
                var customer = await _bankEmployeeAccountService.GetCustomers(id);
                _logger.LogInformation("Retrieved Employees successfully.");
                return customer;
            }
            catch(NoCustomersFoundException ncfe)
            {
                return NotFound(ncfe.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }
        [Authorize(Roles = "BankEmployee")]
        [Route("ApproveAccountCreation")]
        [HttpPost]
        public async Task<ActionResult<Accounts>> ApproveAccountCreation(long accountNumber)
        {
            try
            {
                var result = await _bankEmployeeAccountService.ApproveAccountCreation(accountNumber);
                if (result)
                    return Ok($"Account creation approved for account number: {accountNumber}");
                else
                    return NotFound($"Account with account number {accountNumber} not found or not pending approval.");
            }
            catch (AccountApprovalException ex)
            {
                _logger.LogError($"Error approving account creation for account number {accountNumber}: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error approving account creation for account number {accountNumber}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [Authorize(Roles = "BankEmployee")]
        [Route("ApproveAccountDeletion")]
        [HttpPost]
        public async Task<ActionResult<Accounts>> ApproveAccountDeletion(long accountNumber)
        {
            try
            {
                var result = await _bankEmployeeAccountService.ApproveAccountDeletion(accountNumber);
                if (result)
                    return Ok($"Account deletion approved for account number: {accountNumber}");
                else
                    return NotFound($"Account with account number {accountNumber} not found or not pending deletion.");
            }
            catch (AccountApprovalException ex)
            {
                _logger.LogError($"Error approving account deletion for account number {accountNumber}: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error approving account deletion for account number {accountNumber}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [Authorize(Roles = "BankEmployee")]
        [Route("GetPendingAccounts")]
        [HttpGet]
        public async Task<ActionResult<Accounts>> GetPendingAccounts()
        {
            try
            {
                var pendingAccounts = await _bankEmployeeAccountService.GetPendingAccounts();
                return Ok(pendingAccounts);
            }
            catch (AccountFetchException ex)
            {
                _logger.LogError($"Error fetching pending accounts: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching pending accounts: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [Authorize(Roles = "BankEmployee")]
        [Route("GetPendingDeletion")]
        [HttpGet]
        public async Task<ActionResult<Accounts>> GetPendingDeletionAccounts()
        {
            try
            {
                var pendingDeletionAccounts = await _bankEmployeeAccountService.GetPendingDeletionAccounts();
                return Ok(pendingDeletionAccounts);
            }
            catch (AccountFetchException ex)
            {
                _logger.LogError($"Error fetching accounts pending deletion: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching accounts pending deletion: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
