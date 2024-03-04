using Microsoft.AspNetCore.Mvc;
using Capstone_Project.Models;
using Capstone_Project.Services;
using Capstone_Project.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Capstone_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminAvailableLoansController : ControllerBase
    {
        private readonly IAdminAvailableLoansService _adminAvailableLoansService;
        private readonly ILogger<AdminAvailableLoansController> _logger;

        public AdminAvailableLoansController(
            IAdminAvailableLoansService adminAvailableLoansService,
            ILogger<AdminAvailableLoansController> logger)
        {
            _adminAvailableLoansService = adminAvailableLoansService;
            _logger = logger;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddLoan([FromBody] AvailableLoans loan)
        {
            try
            {
                _logger.LogInformation("Adding loan.");
                var addedLoan = await _adminAvailableLoansService.AddLoan(loan);
                return Ok(addedLoan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding loan via API.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
