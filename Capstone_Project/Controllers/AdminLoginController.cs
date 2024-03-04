using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Capstone_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminLoginController : ControllerBase
    {
        private readonly IAdminLoginService _adminLoginService;
        private readonly ILogger<AdminLoginController> _logger;

        public AdminLoginController(IAdminLoginService adminLoginService, ILogger<AdminLoginController> logger)
        {
            _adminLoginService = adminLoginService;
            _logger = logger;
        }
        [Route("AdminLogin")]
        [HttpPost]
        public async Task<ActionResult<Admin>> Login(LoginUserDTO loginUserDTO)
        {
            try
            {
                var user = await _adminLoginService.Login(loginUserDTO);
                _logger.LogInformation("Login Successful.");
                return Ok(user);
            }
            catch (ValidationNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidUserException)
            {
                return Unauthorized("Invalid email or password.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error logging in: {ex.Message}");
                return StatusCode(500, "An error occurred while processing the login request.");
            }
        }
        [Route("AdminRegister")]
        [HttpPost]
        public async Task<ActionResult<Admin>> Register(RegisterAdminDTO registerAdminDTO)
        {
            try
            {
                var user = await _adminLoginService.Register(registerAdminDTO);
                _logger.LogInformation("Register successfull.");
                return Ok(user);
            }
            catch (ValidationNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error registering user: {ex.Message}");
                return StatusCode(500, "An error occurred while processing the registration request.");
            }
        }
    }
}
