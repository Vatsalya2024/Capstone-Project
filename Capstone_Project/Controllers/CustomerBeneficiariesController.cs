using System;
using System.Threading.Tasks;
using Capstone_Project.Interfaces;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Services;
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

        [Route("Add Beneficiary")]
        [HttpPost]
        public async Task<IActionResult> AddBeneficiary(BeneficiaryDTO beneficiaryDTO)
        {
            try
            {
                await _customerBeneficiaryService.AddBeneficiary(beneficiaryDTO);
                return Ok("Beneficiary added successfully.");
            }
            catch (NoCustomersFoundException ex)
            {
                _logger.LogError(ex, "Error adding beneficiary: No customer found.");
                return NotFound(ex.Message);
            }
            catch (NoBranchesFoundException ex)
            {
                _logger.LogError(ex, "Error adding beneficiary: Branch not found.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding beneficiary.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [Route("Get Bank Branches By Bank Name")]
        [HttpGet]
        public async Task<IActionResult> GetBranchesByBank(string bankName)
        {
            try
            {
                var branches = await _customerBeneficiaryService.GetBranchesByBank(bankName);
                return Ok(branches);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching branches by bank.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [Route("Get IFSC By Branc Name")]
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
    }
}
