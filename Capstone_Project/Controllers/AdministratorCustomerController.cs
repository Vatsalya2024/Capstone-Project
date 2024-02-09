using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Capstone_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdministratorCustomerController : ControllerBase
    {
        private readonly IAdministratorCustomerManagementService _customerManagementService;
        private readonly ILogger<AdministratorCustomerController> _logger;

        public AdministratorCustomerController(IAdministratorCustomerManagementService customerManagementService, ILogger<AdministratorCustomerController> logger)
        {
            _customerManagementService = customerManagementService;
            _logger = logger;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _customerManagementService.GetAllUsers();
                if (users != null)
                {
                    return Ok(users);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving users: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var user = await _customerManagementService.GetUser(id);
                if (user != null)
                {
                    return Ok(user);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving user: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("deactivate/{customerId}")]
        public async Task<IActionResult> DeactivateUser(int customerId)
        {
            var user = await _customerManagementService.DeactivateUser(customerId);
            if (user != null)
            {
                return Ok($"User with ID {customerId} deactivated successfully.");
            }
            else
            {
                return NotFound($"User with ID {customerId} not found or unable to deactivate.");
            }
        }

        [HttpPut("activate/{customerId}")]
        public async Task<IActionResult> ActivateUser(int customerId)
        {
            var user = await _customerManagementService.ActivateUser(customerId);
            if (user != null)
            {
                return Ok($"User with ID {customerId} activated successfully.");
            }
            else
            {
                return NotFound($"User with ID {customerId} not found or unable to activate.");
            }
        }

        [HttpPut("update/name/{customerId}")]
        public async Task<IActionResult> UpdateCustomerName(int customerId, AdminUpdateCustomerNameDTO nameDTO)
        {
            var user = await _customerManagementService.UpdateCustomerName(customerId, nameDTO);
            if (user != null)
            {
                return Ok($"Name for user with ID {customerId} updated successfully.");
            }
            else
            {
                return NotFound($"User with ID {customerId} not found or unable to update name.");
            }
        }

        [HttpPut("update/contact/{customerId}")]
        public async Task<IActionResult> UpdateCustomerContact(int customerId, AdminUpdateCustomerContactDTO contactDTO)
        {
            var user = await _customerManagementService.UpdateCustomerContact(customerId, contactDTO);
            if (user != null)
            {
                return Ok($"Contact information for user with ID {customerId} updated successfully.");
            }
            else
            {
                return NotFound($"User with ID {customerId} not found or unable to update contact information.");
            }
        }

        [HttpPut("update/details/{customerId}")]
        public async Task<IActionResult> UpdateCustomerDetails(int customerId, AdminUpdateCustomerDetailsDTO detailsDTO)
        {
            var user = await _customerManagementService.UpdateCustomerDetails(customerId, detailsDTO);
            if (user != null)
            {
                return Ok($"Details for user with ID {customerId} updated successfully.");
            }
            else
            {
                return NotFound($"User with ID {customerId} not found or unable to update details.");
            }
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateCustomer(RegisterCustomerDTO customerDTO)
        {
            try
            {
                var createdCustomer = await _customerManagementService.CreateCustomer(customerDTO);
                if (createdCustomer != null)
                {
                    return Ok("Customer created successfully.");
                }
                else
                {
                    return StatusCode(500, "Failed to create customer.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating customer: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
