﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
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

        [HttpPost("approve-creation/{accountNumber}")]
        public async Task<IActionResult> ApproveAccountCreation(long accountNumber)
        {
            try
            {
                var result = await _bankEmployeeAccountService.ApproveAccountCreation(accountNumber);
                if (result)
                    return Ok($"Account creation approved for account number: {accountNumber}");
                else
                    return NotFound($"Account with account number {accountNumber} not found or not pending approval.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error approving account creation for account number {accountNumber}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("approve-deletion/{accountNumber}")]
        public async Task<IActionResult> ApproveAccountDeletion(long accountNumber)
        {
            try
            {
                var result = await _bankEmployeeAccountService.ApproveAccountDeletion(accountNumber);
                if (result)
                    return Ok($"Account deletion approved for account number: {accountNumber}");
                else
                    return NotFound($"Account with account number {accountNumber} not found or not pending deletion.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error approving account deletion for account number {accountNumber}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("pending-accounts")]
        public async Task<IActionResult> GetPendingAccounts()
        {
            try
            {
                var pendingAccounts = await _bankEmployeeAccountService.GetPendingAccounts();
                return Ok(pendingAccounts);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching pending accounts: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("pending-deletion-accounts")]
        public async Task<IActionResult> GetPendingDeletionAccounts()
        {
            try
            {
                var pendingDeletionAccounts = await _bankEmployeeAccountService.GetPendingDeletionAccounts();
                return Ok(pendingDeletionAccounts);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching accounts pending deletion: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}