using System;
using System.Threading.Tasks;
using Capstone_Project.Exceptions;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Capstone_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerTransactionController : ControllerBase
    {
        private readonly ILogger<CustomerTransactionController> _logger;
        private readonly ITransactionService _transactionService;

        public CustomerTransactionController(
            ILogger<CustomerTransactionController> logger,
            ITransactionService transactionService)
        {
            _logger = logger;
            _transactionService = transactionService;
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit(DepositDTO depositDTO)
        {
            try
            {
                var result = await _transactionService.Deposit(depositDTO);
                return Ok(new { Message = result });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument exception occurred during deposit.");
                return BadRequest(new { ErrorMessage = ex.Message });
            }
            catch (NotSufficientBalanceException ex)
            {
                _logger.LogError(ex, "Not sufficient balance exception occurred during deposit.");
                return BadRequest(new { ErrorMessage = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during deposit.");
                return StatusCode(500, new { ErrorMessage = "Internal server error occurred." });
            }
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw(WithdrawalDTO withdrawalDTO)
        {
            try
            {
                var result = await _transactionService.Withdraw(withdrawalDTO);
                return Ok(new { Message = result });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument exception occurred during withdrawal.");
                return BadRequest(new { ErrorMessage = ex.Message });
            }
            catch (NotSufficientBalanceException ex)
            {
                _logger.LogError(ex, "Not sufficient balance exception occurred during withdrawal.");
                return BadRequest(new { ErrorMessage = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during withdrawal.");
                return StatusCode(500, new { ErrorMessage = "Internal server error occurred." });
            }
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer(TransferDTO transferDTO)
        {
            try
            {
                var result = await _transactionService.Transfer(transferDTO);
                return Ok(new { Message = result });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument exception occurred during transfer.");
                return BadRequest(new { ErrorMessage = ex.Message });
            }
            catch (NotSufficientBalanceException ex)
            {
                _logger.LogError(ex, "Not sufficient balance exception occurred during transfer.");
                return BadRequest(new { ErrorMessage = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during transfer.");
                return StatusCode(500, new { ErrorMessage = "Internal server error occurred." });
            }

        }
        [Route("Last 10 Transactions")]
        [HttpGet]
        public async Task<IActionResult> GetLast10Transactions(long accountNumber)
        {
            try
            {
                var transactions = await _transactionService.GetLast10Transactions(accountNumber);
                return Ok(transactions);
            }
            catch (NoTransactionsException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving last 10 transactions.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [Route("Last Month Transactions")]
        [HttpGet]
        public async Task<IActionResult> GetLastMonthTransactions(long accountNumber)
        {
            try
            {
                var transactions = await _transactionService.GetLastMonthTransactions(accountNumber);
                return Ok(transactions);
            }
            catch (NoTransactionsException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving last month transactions.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [Route("Transactions Between Dates")]
        [HttpGet]
        public async Task<IActionResult> GetTransactionsBetweenDates(long accountNumber, DateTime startDate, DateTime endDate)
        {
            try
            {
                var transactions = await _transactionService.GetTransactionsBetweenDates(accountNumber, startDate, endDate);
                return Ok(transactions);
            }
            catch (NoTransactionsException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving transactions between dates.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
