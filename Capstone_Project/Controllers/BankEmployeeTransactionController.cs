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
    public class BankEmployeeTransactionController : ControllerBase
    {
        private readonly IBankEmployeeTransactionService _bankEmployeeTransactionService;
        private readonly ILogger<BankEmployeeTransactionController> _logger;

        public BankEmployeeTransactionController(IBankEmployeeTransactionService bankEmployeeTransactionService, ILogger<BankEmployeeTransactionController> logger)
        {
            _bankEmployeeTransactionService = bankEmployeeTransactionService;
            _logger = logger;
        }

        [Authorize(Roles ="BankEmployee")]
        [Route("GetAllTransactions")]
        [HttpGet]
        public async Task<ActionResult<List<Transactions>?>> GetAllTransactions()
        {
            try
            {
                var transactions = await _bankEmployeeTransactionService.GetAllTransactions();
                _logger.LogInformation("Retrieved Transaction successfully.");
                return transactions;
            }
            catch (BankTransactionServiceException ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all transactions");
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error occurred while retrieving all transactions");
                return StatusCode(500, "Internal server error");
            }
        }
        [Authorize(Roles = "BankEmployee")]
        [Route("GetTransactionByAccountNumber")]
        [HttpGet]
        public async Task<ActionResult<List<Transactions>?>> GetTransactionsByAccountNumber(long accountNumber)
        {
            try
            {
                var transactions = await _bankEmployeeTransactionService.GetTransactionsByAccountNumber(accountNumber);
                return transactions;
            }
            catch (BankTransactionServiceException ex)
            {
                _logger.LogError(ex, $"Bank transaction service error occurred while retrieving transactions for account number: {accountNumber}");
                return StatusCode(500, ex.Message);
            }
            catch (NoAccountsFoundException ex)
            {
                _logger.LogError(ex, $"No accounts found for account number: {accountNumber}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unhandled error occurred while retrieving transactions for account number: {accountNumber}");
                return StatusCode(500, "Internal server error");
            }
        }
        [Authorize(Roles = "BankEmployee")]
        [Route("TotalInbound")]
        [HttpGet]
        public async Task<ActionResult<double>> GetTotalInboundTransactions(long accountNumber)
        {
            try
            {
                var totalInboundAmount = await _bankEmployeeTransactionService.GetTotalInboundTransactions(accountNumber);
                return Ok(totalInboundAmount);
            }
            catch (BankTransactionServiceException ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving total inbound transactions for account number: {accountNumber}");
                return StatusCode(500, ex.Message);
            }
            catch (NoAccountsFoundException ex)
            {
                _logger.LogError(ex, $"No accounts found for account number:: {accountNumber}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unhandled error occurred while retrieving total inbound transactions for account number: {accountNumber}");
                return StatusCode(500, "Internal server error");
            }
        }
        [Authorize(Roles = "BankEmployee")]
        [Route("TotalOutbound")]
        [HttpGet]
        public async Task<ActionResult<double>> GetTotalOutboundTransactions(long accountNumber)
        {
            try
            {
                var totalOutboundAmount = await _bankEmployeeTransactionService.GetTotalOutboundTransactions(accountNumber);
                return Ok(totalOutboundAmount);
            }
            catch (BankTransactionServiceException ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving total outbound transactions for account number: {accountNumber}");
                return StatusCode(500, ex.Message);
            }
            catch (NoAccountsFoundException ex)
            {
                _logger.LogError(ex, $"No transactions found for account number: {accountNumber}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unhandled error occurred while retrieving total outbound transactions for account number: {accountNumber}");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
