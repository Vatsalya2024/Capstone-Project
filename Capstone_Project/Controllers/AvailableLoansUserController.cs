using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Microsoft.Extensions.Logging;

namespace Capstone_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvailableLoansUserController : ControllerBase
    {
        private readonly IAvailableLoansUserService _availableLoansUserService;
        private readonly ILogger<AvailableLoansUserController> _logger;

        public AvailableLoansUserController(
            IAvailableLoansUserService availableLoansUserService,
            ILogger<AvailableLoansUserController> logger)
        {
            _availableLoansUserService = availableLoansUserService;
            _logger = logger;
        }

        [HttpGet("getAllLoans")]
        public async Task<IActionResult> GetAllLoans()
        {
            try
            {
                _logger.LogInformation("Retrieving all loans.");
                var loans = await _availableLoansUserService.GetAllLoans();
                if (loans == null || loans.Count == 0)
                {
                    return NotFound("No loans found.");
                }
                return Ok(loans);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all loans via API.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
