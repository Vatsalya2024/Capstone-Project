using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Capstone_Project.Models;
using Capstone_Project.Services;
using Capstone_Project.Interfaces;

namespace Capstone_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminAvailableLoansController : ControllerBase
    {
        private readonly IAdminAvailableLoansService _adminAvailableLoansService;

        public AdminAvailableLoansController(IAdminAvailableLoansService adminAvailableLoansService)
        {
            _adminAvailableLoansService = adminAvailableLoansService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddLoan([FromBody] AvailableLoans loan)
        {
            try
            {
                var addedLoan = await _adminAvailableLoansService.AddLoan(loan);
                return Ok(addedLoan);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
