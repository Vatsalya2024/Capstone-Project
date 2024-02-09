using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        //[HttpPost("Register")]
        //public async Task<ActionResult<LoginUserDTO>> Register(RegisterCustomerDTO user)
        //{
        //    var result = await _customerLoginService.Register(user);
        //    return Ok(result);
        //}

        //[HttpPost("Login")]
        //public async Task<ActionResult<LoginUserDTO>> Login(LoginUserDTO user)
        //{
        //    try
        //    {
        //        var result = await _customerLoginService.Login(user);
        //        return Ok(result);
        //    }
        //    catch (InvalidUserException iuse)
        //    {
        //        _logger.LogError(iuse.Message);
        //        return Unauthorized("Invalid username or password");
        //    }
        //}

        [HttpPost("Register")]
        public async Task<ActionResult<LoginUserDTO>> Register(RegisterCustomerDTO user)
        {
            var result = await _customerLoginService.Register(user);
            return Ok(result);
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
        }


        [Authorize]
        [HttpGet("GetAllCustomer")]
        public async Task<ActionResult<List<Customers>>> GetCustomers()
        {
            var customers = await _customerAdminService.GetCustomersListasync();
            return Ok(customers);
        }

        [Authorize(Roles = "Customer")]
        [HttpPut("UpdatePhone")]
        public async Task<ActionResult<Customers>> UpdatePhone(UpdateCustomerPhoneDTO updateCustomerPhoneDTO)
        {
            var customer = await _customerAdminService.ChangeCustomerPhoneAsync(updateCustomerPhoneDTO.CustomerID, updateCustomerPhoneDTO.Phone);
            return Ok(customer);
        }

        [HttpPut("UpdateName")]
        public async Task<ActionResult<Customers>> UpdateCustomerName(UpdateCustomerNameDTO updateCustomerNameDTO)
        {
            var customer = await _customerAdminService.ChangeCustomerName(updateCustomerNameDTO.CustomerID, updateCustomerNameDTO.Name);
            return Ok(customer);
        }

        [HttpPut("UpdateAddress")]
        public async Task<ActionResult<Customers>> UpdateCustomerAddress(UpdateCustomerAddressDTO updateCustomerAddressDTO)
        {
            var customer = await _customerAdminService.ChangeCustomerAddress(updateCustomerAddressDTO.CustomerID, updateCustomerAddressDTO.Address);
            return Ok(customer);
        }

        [HttpGet("GetCustomerById")]
        public async Task<ActionResult<Customers>> GetCustomerByID(int id)
        {
            var customer = await _customerAdminService.GetCustomers(id);
            if (customer == null)
            {
                return NotFound();
            }
            return customer;
        }
        [HttpDelete("DeleteCustomer/{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<Customers>> DeleteCustomer(int id)
        {
            try
            {
                var result = await _customerAdminService.DeleteCustomers(id);
                return Ok(result);
            }
            catch (NoCustomersFoundException ncf)
            {
                _logger.LogError(ncf.Message);
                return NotFound("Customer not found");
            }
        }
        [HttpPut("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(UpdateCustomerPasswordDTO updatePasswordDTO)
        {
            try
            {
                // Call the service method to update the customer's password
                bool passwordUpdated = await _customerAdminService.UpdateCustomerPassword(updatePasswordDTO.Email, updatePasswordDTO.NewPassword);

                if (passwordUpdated)
                {
                    return Ok("Password updated successfully");
                }
                else
                {
                    return NotFound("Customer not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

