using Capstone_Project.Exceptions;
using Capstone_Project.Interfaces;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Capstone_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankEmployeeLoginController : ControllerBase
    {
        private readonly IBankEmployeeLoginService _bankEmployeeService;

        public BankEmployeeLoginController(IBankEmployeeLoginService bankEmployeeService)
        {
            _bankEmployeeService = bankEmployeeService;
        }
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUserDTO)
        {
            try
            {
                var result = await _bankEmployeeService.Login(loginUserDTO);
                return Ok(result);
            }
            catch (InvalidUserException)
            {
                return Unauthorized("Invalid username or password");
            }
            catch (DeactivatedUserException)
            {
                return Unauthorized("User deactivated");
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterBankEmployeeDTO registerBankEmployeeDTO)
        {
            try
            {
                var result = await _bankEmployeeService.Register(registerBankEmployeeDTO);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
