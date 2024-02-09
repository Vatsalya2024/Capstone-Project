using System;
using System.Threading.Tasks;
using Capstone_Project.Interfaces;
using Capstone_Project.Models.DTOs;
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
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument exception occurred during deposit.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during deposit.");
                return StatusCode(500, "Internal server error occurred.");
            }
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw(WithdrawalDTO withdrawalDTO)
        {
            try
            {
                var result = await _transactionService.Withdraw(withdrawalDTO);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument exception occurred during withdrawal.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during withdrawal.");
                return StatusCode(500, "Internal server error occurred.");
            }
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer(TransferDTO transferDTO)
        {
            try
            {
                var result = await _transactionService.Transfer(transferDTO);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument exception occurred during transfer.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during transfer.");
                return StatusCode(500, "Internal server error occurred.");
            }
        }
    }
}
