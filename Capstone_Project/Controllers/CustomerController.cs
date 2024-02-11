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
using Microsoft.AspNetCore.Mvc;

namespace Capstone_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        [HttpPost("Register")]
        public async Task<ActionResult<LoginUserDTO>> Register(RegisterCustomerDTO user)
        {
            try
            {
                var result = await _customerLoginService.Register(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<LoginUserDTO>> Login(LoginUserDTO user)
        {
            try
            {
                var result = await _customerLoginService.Login(user);
                return Ok(result);
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

        //[Authorize]
        //[HttpGet("GetAllCustomer")]
        //public async Task<ActionResult<List<Customers>>> GetCustomers()
        //{
        //    try
        //    {
        //        var customers = await _customerAdminService.GetCustomersListasync();
        //        return Ok(customers);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        //[Authorize(Roles = "Customer")]
        //[HttpPut("UpdatePhone")]
        //public async Task<ActionResult<Customers>> UpdatePhone(UpdateCustomerPhoneDTO updateCustomerPhoneDTO)
        //{
        //    try
        //    {
        //        var customer = await _customerAdminService.ChangeCustomerPhoneAsync(updateCustomerPhoneDTO.CustomerID, updateCustomerPhoneDTO.Phone);
        //        return Ok(customer);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        //[HttpPut("UpdateName")]
        //public async Task<ActionResult<Customers>> UpdateCustomerName(UpdateCustomerNameDTO updateCustomerNameDTO)
        //{
        //    try
        //    {
        //        var customer = await _customerAdminService.ChangeCustomerName(updateCustomerNameDTO.CustomerID, updateCustomerNameDTO.Name);
        //        return Ok(customer);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        //[HttpPut("UpdateAddress")]
        //public async Task<ActionResult<Customers>> UpdateCustomerAddress(UpdateCustomerAddressDTO updateCustomerAddressDTO)
        //{
        //    try
        //    {
        //        var customer = await _customerAdminService.ChangeCustomerAddress(updateCustomerAddressDTO.CustomerID, updateCustomerAddressDTO.Address);
        //        return Ok(customer);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        [HttpPut("{id}/change-phone")]
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

        [HttpPut("{id}/change-name")]
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

        [HttpPut("{id}/change-address")]
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


        //[HttpGet("GetCustomerById")]
        //public async Task<ActionResult<Customers>> GetCustomerByID(int id)
        //{
        //    try
        //    {
        //        var customer = await _customerAdminService.GetCustomers(id);
        //        if (customer == null)
        //        {
        //            return NotFound();
        //        }
        //        return customer;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        //[HttpDelete("DeleteCustomer/{id}")]
        //[Authorize(Roles = "Customer")]
        //public async Task<ActionResult<Customers>> DeleteCustomer(int id)
        //{
        //    try
        //    {
        //        var result = await _customerAdminService.DeleteCustomers(id);
        //        return Ok(result);
        //    }
        //    catch (NoCustomersFoundException ncf)
        //    {
        //        _logger.LogError(ncf.Message);
        //        return NotFound("Customer not found");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        [HttpDelete("delete-customer/{id}")]
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



        //[HttpPut("UpdatePassword")]
        //public async Task<ActionResult> UpdatePassword(UpdateCustomerPasswordDTO updatePasswordDTO)
        //{
        //    try
        //    {
        //        bool passwordUpdated = await _customerAdminService.UpdateCustomerPassword(updatePasswordDTO.Email, updatePasswordDTO.NewPassword);

        //        if (passwordUpdated)
        //        {
        //            return Ok("Password updated successfully");
        //        }
        //        else
        //        {
        //            return NotFound("Customer not found");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        [HttpPost("update-password")]
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

    }
}
