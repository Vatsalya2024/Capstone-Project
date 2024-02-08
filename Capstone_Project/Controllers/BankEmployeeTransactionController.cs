using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Services;
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

        [HttpGet("all")]
        public async Task<ActionResult<List<Transactions>?>> GetAllTransactions()
        {
            try
            {
                var transactions = await _bankEmployeeTransactionService.GetAllTransactions();
                return transactions;
            }
            catch (BankTransactionServiceException ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all transactions");
                return StatusCode(500, "Internal server error occurred");
            }
        }

        [HttpGet("{accountNumber}/transactions")]
        public async Task<ActionResult<List<Transactions>?>> GetTransactionsByAccountNumber(long accountNumber)
        {
            try
            {
                var transactions = await _bankEmployeeTransactionService.GetTransactionsByAccountNumber(accountNumber);
                if (transactions == null)
                {
                    return NotFound();
                }
                return transactions;
            }
            catch (BankTransactionServiceException ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving transactions for account number: {accountNumber}");
                return StatusCode(500, "Internal server error occurred");
            }
        }

        [HttpGet("{accountNumber}/total-inbound")]
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
                return StatusCode(500, "Internal server error occurred");
            }
        }

        [HttpGet("{accountNumber}/total-outbound")]
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
                return StatusCode(500, "Internal server error occurred");
            }
        }


    }
}
