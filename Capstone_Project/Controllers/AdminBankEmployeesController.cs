using Capstone_Project.Interfaces;
using Capstone_Project.Mappers;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        [HttpGet]
        public async Task<ActionResult<List<BankEmployees>>> GetAllEmployees()
        {
            var employees = await _bankEmployeeService.GetAllEmployees();
            if (employees == null)
            {
                return NotFound();
            }
            return employees;
        }

        [HttpGet("{employeeId}")]
        public async Task<ActionResult<BankEmployees>> GetEmployee(int employeeId)
        {
            var employee = await _bankEmployeeService.GetEmployee(employeeId);
            if (employee == null)
            {
                return NotFound();
            }
            return employee;
        }

        [HttpPost("{employeeId}/activate")]
        public async Task<ActionResult<BankEmployees>> ActivateEmployee(int employeeId)
        {
            var activatedEmployee = await _bankEmployeeService.ActivateEmployee(employeeId);
            if (activatedEmployee == null)
            {
                return NotFound();
            }
            return activatedEmployee;
        }

        [HttpPost("{employeeId}/deactivate")]
        public async Task<ActionResult<BankEmployees>> DeactivateEmployee(int employeeId)
        {
            var deactivatedEmployee = await _bankEmployeeService.DeactivateEmployee(employeeId);
            if (deactivatedEmployee == null)
            {
                return NotFound();
            }
            return deactivatedEmployee;
        }

        [HttpPost]
        public async Task<ActionResult<BankEmployees>> CreateBankEmployee(RegisterBankEmployeeDTO employeeDTO)
        {
            try
            {
                var addedBankEmployee = await _bankEmployeeService.CreateBankEmployee(employeeDTO);
                if (addedBankEmployee == null)
                {
                    return BadRequest();
                }
                return addedBankEmployee;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating bank employee: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPut("update/{employeeId}")]
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
            catch (Exception ex)
            {
                _logger.LogError($"Error updating employee: {ex.Message}");
                return StatusCode(500, "An error occurred while updating the employee.");
            }
        }
    }
}
