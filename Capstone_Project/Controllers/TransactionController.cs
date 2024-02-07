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
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("deposit")]
        public async Task<ActionResult<Transactions>> Deposit(DepositDTO depositDTO)
        {
            try
            {
                var result = await _transactionService.Deposit(depositDTO);
                if (result)
                    return Ok("Deposit successful.");
                else
                    return BadRequest("Deposit failed.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("withdraw")]
        public async Task<ActionResult<Transactions>> Withdraw(WithdrawalDTO withdrawalDTO)
        {
            try
            {
                var result = await _transactionService.Withdraw(withdrawalDTO);
                if (result)
                    return Ok("Withdrawal successful.");
                else
                    return BadRequest("Withdrawal failed.");
            }
            catch (NotSufficientBalanceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("transfer")]
        public async Task<ActionResult<Transactions>> Transfer(TransferDTO transferDTO)
        {
            try
            {
                var result = await _transactionService.Transfer(transferDTO);
                if (result)
                    return Ok("Transfer successful.");
                else
                    return BadRequest("Transfer failed.");
            }
            catch (NotSufficientBalanceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

