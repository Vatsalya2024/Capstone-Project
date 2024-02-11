using Capstone_Project.Interfaces;
using Capstone_Project.Mappers;
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
        [Route("Get All Employees")]
        [HttpGet]
        public async Task<ActionResult<List<BankEmployees>>> GetAllEmployees()
        {
            try
            {
                var employees = await _bankEmployeeService.GetAllEmployees();
                
                return employees;
            }
            catch (EmployeeNotFoundException ex)
            {
                _logger.LogError($"No employees found: {ex.Message}");
                return NotFound("No employees found.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving employees: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [Route("get employee by id")]
        [HttpGet]
        public async Task<ActionResult<BankEmployees>> GetEmployee(int employeeId)
        {
            try
            {
                var employee = await _bankEmployeeService.GetEmployee(employeeId);
               
                return employee;
            }
            catch (EmployeeNotFoundException ex)
            {
                _logger.LogError($"Employee not found: {ex.Message}");
                return NotFound($"Employee with ID {employeeId} not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving employee: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [Route("Activate Employee")]
        [HttpPost]
        public async Task<ActionResult<BankEmployees>> ActivateEmployee(int employeeId)
        {
            try
            {
                var activatedEmployee = await _bankEmployeeService.ActivateEmployee(employeeId);
                
                return activatedEmployee;
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
            catch (Exception ex)
            {
                _logger.LogError($"Error activating employee: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [Route("Deactivate Employee")]
        [HttpPost]
        public async Task<ActionResult<BankEmployees>> DeactivateEmployee(int employeeId)
        {
            try
            {
                var deactivatedEmployee = await _bankEmployeeService.DeactivateEmployee(employeeId);
                
                return deactivatedEmployee;
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
            catch (Exception ex)
            {
                _logger.LogError($"Error deactivating employee: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [Route("Register Bank Employee")]
        [HttpPost]
        public async Task<ActionResult<BankEmployees>> CreateBankEmployee(RegisterBankEmployeeDTO employeeDTO)
        {
            try
            {
                var addedBankEmployee = await _bankEmployeeService.CreateBankEmployee(employeeDTO);
               
                return addedBankEmployee;
            }
            catch (BankEmployeeCreationException ex)
            {
                _logger.LogError($"Error creating bank employee: {ex.Message}");
                return StatusCode(500, "Error creating bank employee");
            }
        }
        [Route("Update Bank Employee")]
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

                // Update the employee
                UpdateBankEmployeeByAdminMapper.MapToBankEmployee(updateDTO, employee);

                // Save the changes
                // Assuming you have a method to update the employee in your repository
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
