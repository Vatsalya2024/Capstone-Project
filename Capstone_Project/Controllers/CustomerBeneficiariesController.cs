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

        [HttpPost("add")]
        public async Task<IActionResult> AddBeneficiary([FromBody] BeneficiaryDTO beneficiaryDTO)
        {
            try
            {
                var result = await _customerBeneficiaryService.AddBeneficiary(beneficiaryDTO);
                if (result)
                {
                    _logger.LogInformation("Beneficiary added successfully.");
                    return Ok("Beneficiary added successfully.");
                }
                else
                {
                    _logger.LogWarning("Failed to add beneficiary.");
                    return BadRequest("Failed to add beneficiary.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding beneficiary.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("branches/{bankName}")]
        public async Task<IActionResult> GetBranchesByBank(string bankName)
        {
            try
            {
                var branches = await _customerBeneficiaryService.GetBranchesByBank(bankName);
                _logger.LogInformation("Branches fetched successfully by bank.");
                return Ok(branches);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching branches by bank.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("ifsc/{branchName}")]
        public async Task<IActionResult> GetIFSCByBranch(string branchName)
        {
            try
            {
                var ifsc = await _customerBeneficiaryService.GetIFSCByBranch(branchName);
                if (ifsc != null)
                {
                    _logger.LogInformation("IFSC fetched successfully for the branch.");
                    return Ok(ifsc);
                }
                else
                {
                    _logger.LogWarning("IFSC not found for the specified branch name.");
                    return NotFound("IFSC not found for the specified branch name.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching IFSC by branch.");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
