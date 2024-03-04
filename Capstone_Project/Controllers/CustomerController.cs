using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone_Project.Exceptions;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Capstone_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("ReactPolicy")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerLoginService _customerLoginService;
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerAdminService _customerAdminService;

        public CustomerController(ICustomerLoginService customerLoginService, ILogger<CustomerController> logger, ICustomerAdminService customerAdminService)
        {
            _customerLoginService = customerLoginService;
            _logger = logger;
            _customerAdminService = customerAdminService;
        }
        [Route("Register")]
        [HttpPost]
        public async Task<ActionResult<LoginUserDTO>> Register(RegisterCustomerDTO user)
        {
            try
            {
                var result = await _customerLoginService.Register(user);
                return Ok(result);
            }
            catch(ValidationNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }
        [Route("Login")]
        [HttpPost]
        public async Task<ActionResult<LoginUserDTO>> Login(LoginUserDTO user)
        {
            try
            {
                var result = await _customerLoginService.Login(user);
                return Ok(result);
            }
            catch (ValidationNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            catch (InvalidUserException iuse)
            {
                _logger.LogError(iuse.Message);
                return Unauthorized("Invalid username or password");
            }
            catch (DeactivatedUserException)
            {
                return Unauthorized("User deactivated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }
        [Route("ResetPassword")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(string email, string newPassword, string confirmPassword)
        {
            try
            {
                
                var resetSuccessful = await _customerAdminService.ResetPassword(email, newPassword, confirmPassword);

                if (resetSuccessful)
                {
                    return Ok("Password reset successfully.");
                }
                else
                {
                    return BadRequest("Password reset failed.");
                }
            }
            catch (PasswordMismatchException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("New password and confirm password do not match.");
            }
            catch (ValidationNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound($"Validation not found for email: {email}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }
        [Authorize(Roles = "Customer")]
        [Route("ChangePhoneNumber")]
        [HttpPut]
        public async Task<IActionResult> ChangeCustomerPhoneAsync(int id, long phone)
        {
            try
            {
                var updatedCustomer = await _customerAdminService.ChangeCustomerPhoneAsync(id, phone);
                return Ok(updatedCustomer);
            }
            catch (NoCustomersFoundException ex)
            {
                _logger.LogError(ex, $"Error changing customer phone: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing customer phone.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [Authorize(Roles = "Customer")]
        [Route("ChangeName")]
        [HttpPut]
        public async Task<IActionResult> ChangeCustomerName(int id, string name)
        {
            try
            {
                var updatedCustomer = await _customerAdminService.ChangeCustomerName(id, name);
                return Ok(updatedCustomer);
            }
            catch (NoCustomersFoundException ex)
            {
                _logger.LogError(ex, $"Error changing customer name: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing customer name.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [Authorize(Roles = "Customer")]
        [Route("ChangeAddress")]
        [HttpPut]
        public async Task<IActionResult> ChangeCustomerAddress(int id, string address)
        {
            try
            {
                var updatedCustomer = await _customerAdminService.ChangeCustomerAddress(id, address);
                return Ok(updatedCustomer);
            }
            catch (NoCustomersFoundException ex)
            {
                _logger.LogError(ex, $"Error changing customer address: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing customer address.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [Authorize(Roles = "Customer")]
        [Route("Delete")]
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                var deletedCustomer = await _customerAdminService.DeleteCustomers(id);
                return Ok(deletedCustomer);
            }
            catch (NoCustomersFoundException ex)
            {
                _logger.LogError(ex, $"Error deleting customer: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting customer.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        
        [Route("UpdatePassword")]
        [HttpPost]
        public async Task<IActionResult> UpdateCustomerPassword(string email, string newPassword)
        {
            try
            {
                var updated = await _customerAdminService.UpdateCustomerPassword(email, newPassword);
                
                    return Ok("Password updated successfully.");
                
            }
            catch (ValidationNotFoundException ex)
            {
                _logger.LogError(ex, $"Error updating password: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating password.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("GetCustomerInfoByEmail")]
        public async Task<ActionResult<Customers>> GetCustomerInfoByEmail(string email)
        {
            try
            {
                var customerInfo = await _customerAdminService.GetCustomerInfoByEmail(email);
                if (customerInfo == null)
                {
                    return NotFound($"Customer with email {email} not found.");
                }
                return Ok(customerInfo);
            }
            catch (ValidationNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (NoCustomersFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
