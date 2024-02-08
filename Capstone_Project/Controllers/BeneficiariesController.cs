
using Capstone_Project.Exceptions;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Capstone_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeneficiariesController : ControllerBase
    {
        private readonly IBeneficiaryService _beneficiaryService;
        private readonly ILogger<BeneficiariesController> _logger;

        public BeneficiariesController(IBeneficiaryService beneficiaryService, ILogger<BeneficiariesController> logger)
        {
            _beneficiaryService = beneficiaryService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<Beneficiaries>> AddBeneficiaryAsync(Beneficiaries beneficiary)
        {
            try
            {
                var addedBeneficiary = await _beneficiaryService.AddBeneficiaryAsync(beneficiary);
                return Ok(addedBeneficiary);
            }
            catch (BeneficiaryServiceException ex)
            {
                _logger.LogError(ex, "Error occurred while adding beneficiary");
                return StatusCode(500, "Error occurred while adding beneficiary");
            }
        }
    }
}
