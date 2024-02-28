using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;

namespace Capstone_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvailableLoansUserController : ControllerBase
    {
        private readonly IAvailableLoansUserService _availableLoansUserService;

        public AvailableLoansUserController(IAvailableLoansUserService availableLoansUserService)
        {
            _availableLoansUserService = availableLoansUserService;
        }

        [HttpGet("getAllLoans")]
        public async Task<IActionResult> GetAllLoans()
        {
            try
            {
                var loans = await _availableLoansUserService.GetAllLoans();
                if (loans == null || loans.Count == 0)
                {
                    return NotFound("No loans found.");
                }
                return Ok(loans);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
