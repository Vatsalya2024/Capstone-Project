using System;
using System.Threading.Tasks;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Capstone_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerBeneficiariesController : ControllerBase
    {
        private readonly ICustomerBeneficiaryService _customerBeneficiaryService;
        private readonly ILogger<CustomerBeneficiariesController> _logger;

        public CustomerBeneficiariesController(
            ICustomerBeneficiaryService customerBeneficiaryService,
            ILogger<CustomerBeneficiariesController> logger)
        {
            _customerBeneficiaryService = customerBeneficiaryService;
            _logger = logger;
        }
        [Authorize(Roles ="Customer")]
        [HttpPost("AddBeneficiary")]
        public async Task<IActionResult> AddBeneficiary(BeneficiaryDTO beneficiaryDTO)
        {
            try
            {
                var beneficiary = await _customerBeneficiaryService.AddBeneficiary(beneficiaryDTO);
                return Ok("Beneficiary added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding beneficiary.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }



        [Authorize(Roles = "Customer")]
        [Route("GetBeneficiaryByCustomerId")]
        [HttpGet]
        public async Task<IActionResult> GetBeneficiaries(int customerId)
        {
            try
            {
                var beneficiaries = await _customerBeneficiaryService.GetBeneficiariesByCustomerID(customerId);
                return Ok(beneficiaries);
            }
            catch (NoCustomersFoundException ex)
            {
                _logger.LogError(ex, "No Customer Found");
                return StatusCode(500, "No Customer Found");
            }
            catch(NoBeneficiariesFoundException ex)
            {
                _logger.LogError(ex, "No Beneficiary found");
                return StatusCode(500, "No Beneficiary Found");
            }
        }



        [Authorize(Roles = "Customer")]
        [Route("GetBankBranchesByBankName")]
        [HttpGet]
        public async Task<IActionResult> GetBranchesByBank(string bankName)
        {
            try
            {
                var branches = await _customerBeneficiaryService.GetBranchesByBank(bankName);
                return Ok(branches);
            }
            catch(NoBranchesFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            catch(NoBanksFoundException nbfe)
            {
                _logger.LogError(nbfe, "Bank Not found");
                return StatusCode(500, "No bank found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching branches by bank.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [Authorize(Roles = "Customer")]
        [Route("GetIFSCByBranchName")]
        [HttpGet]
        public async Task<IActionResult> GetIFSCByBranch(string branchName)
        {
            try
            {
                var ifsc = await _customerBeneficiaryService.GetIFSCByBranch(branchName);
                return Ok(ifsc);
            }
            catch (NoBranchesFoundException ex)
            {
                _logger.LogError(ex, "Error fetching IFSC by branch: Branch not found.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching IFSC by branch.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [Authorize(Roles = "Customer")]
        [HttpPost("TransferToBeneficiary")]
        public async Task<IActionResult> TransferToBeneficiary(BeneficiaryTransferDTO transferDTO)
        {
            try
            {
                var result = await _customerBeneficiaryService.TransferToBeneficiary(transferDTO);
                return Ok(result);
            }
            catch (NoAccountsFoundException ex)
            {
                _logger.LogError(ex, "Error transferring to beneficiary: Account not found.");
                return NotFound(ex.Message);
            }
            catch (NotSufficientBalanceException ex)
            {
                _logger.LogError(ex, "Error transferring to beneficiary: Not sufficient balance.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error transferring to beneficiary.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}
