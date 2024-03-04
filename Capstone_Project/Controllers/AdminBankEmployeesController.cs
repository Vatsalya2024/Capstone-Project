using Capstone_Project.Interfaces;
using Capstone_Project.Mappers;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Capstone_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminBankEmployeesController : ControllerBase
    {
        private readonly IAdministratorBankEmployeeManagementService _bankEmployeeService;
        private readonly ILogger<AdminBankEmployeesController> _logger;

        public AdminBankEmployeesController(IAdministratorBankEmployeeManagementService bankEmployeeService,
            ILogger<AdminBankEmployeesController> logger)
        {
            _bankEmployeeService = bankEmployeeService;
            _logger = logger;
        }
        [Authorize(Roles = "Admin")]
        [Route("GetAllEmployees")]
        [HttpGet]
        public async Task<ActionResult<List<BankEmployees>>> GetAllEmployees()
        {
            try
            {
                var employees = await _bankEmployeeService.GetAllEmployees();
                _logger.LogInformation("Retrieved all employees successfully.");
                return Ok(employees);
            }
            catch (EmployeeNotFoundException ex)
            {
                _logger.LogError($"No employees found: {ex.Message}");
                return NotFound("No employees found.");
            }
            
        }
        [Authorize(Roles = "Admin")]
        [Route("getemployeebyid")]
        [HttpGet]
        public async Task<ActionResult<BankEmployees>> GetEmployee(int employeeId)
        {
            try
            {
                var employee = await _bankEmployeeService.GetEmployee(employeeId);
                _logger.LogInformation("Retrieved employee successfully.");
                return Ok(employee);
            }
            catch (EmployeeNotFoundException ex)
            {
                _logger.LogError($"Employee not found: {ex.Message}");
                return NotFound($"Employee with ID {employeeId} not found.");
            }
           
        }
        [Authorize(Roles = "Admin")]
        [Route("ActivateEmployee")]
        [HttpPost]
        public async Task<ActionResult<BankEmployees>> ActivateEmployee(int employeeId)
        {
            try
            {
                var activatedEmployee = await _bankEmployeeService.ActivateEmployee(employeeId);
                _logger.LogInformation("Activated employee successfully.");
                return Ok(activatedEmployee);
            }
            
            catch (EmployeeNotFoundException ex)
            {
                _logger.LogError($"Employee not found: {ex.Message}");
                return NotFound($"Employee with ID {employeeId} not found.");
            }
            catch (ValidationNotFoundException ex)
            {
                _logger.LogError($"Validation not found: {ex.Message}");
                return NotFound($"Validation for employee with ID {employeeId} not found.");
            }
          
        }
        [Authorize(Roles = "Admin")]
        [Route("DeactivateEmployee")]
        [HttpPost]
        public async Task<ActionResult<BankEmployees>> DeactivateEmployee(int employeeId)
        {
            try
            {
                var deactivatedEmployee = await _bankEmployeeService.DeactivateEmployee(employeeId);
                _logger.LogInformation("Deactivated employee successfully.");
                return Ok(deactivatedEmployee);
            }
            catch (EmployeeNotFoundException ex)
            {
                _logger.LogError($"Employee not found: {ex.Message}");
                return NotFound($"Employee with ID {employeeId} not found.");
            }
            catch (ValidationNotFoundException ex)
            {
                _logger.LogError($"Validation not found: {ex.Message}");
                return NotFound($"Validation for employee with ID {employeeId} not found.");
            }
           
        }
        [Authorize(Roles = "Admin")]
        [Route("RegisterBankEmployee")]
        [HttpPost]
        public async Task<ActionResult<BankEmployees>> CreateBankEmployee(RegisterBankEmployeeDTO employeeDTO)
        {
            try
            {
                var addedBankEmployee = await _bankEmployeeService.CreateBankEmployee(employeeDTO);
                _logger.LogInformation("Registered employee successfully.");
                return Ok(addedBankEmployee);
            }
            catch (BankEmployeeCreationException ex)
            {
                _logger.LogError($"Error creating bank employee: {ex.Message}");
                return StatusCode(500, "Error creating bank employee");
            }
        }
        [Authorize(Roles = "Admin")]
        [Route("UpdateBankEmployee")]
        [HttpPut]
        public async Task<IActionResult> UpdateEmployee(int employeeId, UpdateBankEmployeeByAdminDTO updateDTO)
        {
            try
            {
                var employee = await _bankEmployeeService.GetEmployee(employeeId);
                if (employee == null)
                {
                    return NotFound($"Employee with ID {employeeId} not found.");
                }

                
                UpdateBankEmployeeByAdminMapper.MapToBankEmployee(updateDTO, employee);

                
                var updatedEmployee = await _bankEmployeeService.UpdateEmployee(employee);

                if (updatedEmployee != null)
                {
                    return Ok(updatedEmployee);
                }
                else
                {
                    return StatusCode(500, "Failed to update employee.");
                }
            }
            catch (EmployeeNotFoundException ex)
            {
                _logger.LogError($"Employee not found: {ex.Message}");
                return NotFound($"Employee with ID {employeeId} not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating employee: {ex.Message}");
                return StatusCode(500, "An error occurred while updating the employee.");
            }
        }
    }
}
