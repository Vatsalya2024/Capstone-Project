﻿using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Services;
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
        [Route("Get All Customers")]
        [HttpGet]
        public async Task<ActionResult<Customers>> GetAllUsers()
        {
            try
            {
                var users = await _customerManagementService.GetAllUsers();
               
                    return Ok(users);
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving users: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
       
        [Route("GetCustomerById")]
        [HttpGet]
        public async Task<ActionResult<Customers>> GetUser(int id)
        {
            try
            {
                var user = await _customerManagementService.GetUser(id);
               
                    return Ok(user);
              
            }
            catch (NoCustomersFoundException ex)
            {
                _logger.LogError($"Error retrieving user with ID {id}: {ex.Message}");
                return NotFound($"User with ID {id} not found.");
            }
           
        }
        [Route("Deactivate Customer")]
        [HttpPut]
        public async Task<ActionResult<Customers>> DeactivateUser(int customerId)
        {
            try
            {
                var user = await _customerManagementService.DeactivateUser(customerId);
                
                    return Ok($"User with ID {customerId} deactivated successfully.");
                
            }
            catch (NoCustomersFoundException ex)
            {
                _logger.LogError($"Error deactivating user: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (ValidationNotFoundException ex)
            {
                _logger.LogError($"Error deactivating user: {ex.Message}");
                return NotFound(ex.Message);
            }
            
        }
        [Route("Activate Customer")]
        [HttpPut]
        public async Task<ActionResult<Customers>> ActivateUser(int customerId)
        {
            try
            {
                var user = await _customerManagementService.ActivateUser(customerId);
                
                    return Ok($"User with ID {customerId} activated successfully.");
                
                
            }
            catch (NoCustomersFoundException ex)
            {
                _logger.LogError($"Error activating user: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (ValidationNotFoundException ex)
            {
                _logger.LogError($"Error activating user: {ex.Message}");
                return NotFound(ex.Message);
            }
           
        }

        
        

        [Route("UpdateCustomerName")]
        [HttpPut]
        public async Task<ActionResult<Customers>> UpdateCustomerName(int customerId, AdminUpdateCustomerNameDTO nameDTO)
        {
            try
            {
                var user = await _customerManagementService.UpdateCustomerName(customerId, nameDTO);
                return Ok($"Name for user with ID {customerId} updated successfully.");
            }
            catch (NoCustomersFoundException ex)
            {
                _logger.LogError($"Error updating name for user with ID {customerId}: {ex.Message}");
                return NotFound($"User with ID {customerId} not found.");
            }
           
        }

        [Route("UpdateCustomerContact")]
        [HttpPut]
        public async Task<ActionResult<Customers>> UpdateCustomerContact(int customerId, AdminUpdateCustomerContactDTO contactDTO)
        {
            try
            {
                var user = await _customerManagementService.UpdateCustomerContact(customerId, contactDTO);
                return Ok($"Contact information for user with ID {customerId} updated successfully.");
            }
            catch (NoCustomersFoundException ex)
            {
                _logger.LogError($"Error updating contact information for user with ID {customerId}: {ex.Message}");
                return NotFound($"User with ID {customerId} not found.");
            }
          
        }

        [Route("UpdateCustomerDetails")]
        [HttpPut]
        public async Task<ActionResult<Customers>> UpdateCustomerDetails(int customerId, AdminUpdateCustomerDetailsDTO detailsDTO)
        {
            try
            {
                var user = await _customerManagementService.UpdateCustomerDetails(customerId, detailsDTO);
                return Ok($"Details for user with ID {customerId} updated successfully.");
            }
            catch (NoCustomersFoundException ex)
            {
                _logger.LogError($"Error updating details for user with ID {customerId}: {ex.Message}");
                return NotFound($"User with ID {customerId} not found.");
            }
          
        }

        [Route("Register Customer")]
        [HttpPost]
        public async Task<ActionResult<Customers>> CreateCustomer(RegisterCustomerDTO customerDTO)
        {
            try
            {
                var createdCustomer = await _customerManagementService.CreateCustomer(customerDTO);
                
                    return Ok("Customer created successfully");
               
            }
            catch (CustomerCreationException ex)
            {
                _logger.LogError($"Error creating customer: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

    }
}
