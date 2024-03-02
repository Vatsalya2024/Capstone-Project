using System;
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
    public class BankEmployeeController : ControllerBase
    {
        private readonly IBankEmployeeService _userService;
        private readonly ILogger<BankEmployeeController> _logger;
        private readonly IBankEmployeeService _bankEmployeeService;

        public BankEmployeeController(IBankEmployeeService userService, ILogger<BankEmployeeController> logger, IBankEmployeeService bankEmployeeService)
        {
            _userService = userService;
            _logger = logger;
            _bankEmployeeService = bankEmployeeService;
        }
        [Authorize(Roles = "BankEmployee")]
        [Route("GetAllBankEmployees")]
        [HttpGet]
        public async Task<ActionResult<List<BankEmployees>>> GetAllBankEmployees()
        {
            try
            {
                return await _bankEmployeeService.GetAllBankEmployee();
            }
            catch (NoBankEmployeesFoundException e)
            {
                _logger.LogInformation(e.Message);
                return NotFound(e.Message);
            }
        }

        [Route("GetBankEmployee")]
        [HttpGet]
        public async Task<ActionResult<BankEmployees>> GetBankEmployee(int key)
        {
            try
            {
                return await _bankEmployeeService.GetBankEmployee(key);
            }
            catch (NoBankEmployeesFoundException e)
            {
                _logger.LogInformation(e.Message);
                return NotFound(e.Message);
            }
        }

        [Route("UpdateBankEmployeeName")]
        [HttpPut]
        public async Task<ActionResult<BankEmployees>> UpdateBankEmployeeName(UpdateBankEmployeeNameDTO updateBankEmployeeNameDTO)
        {
            try
            {
                return await _bankEmployeeService.UpdateBankEmployeeName(updateBankEmployeeNameDTO);
            }
            catch (NoBankEmployeesFoundException e)
            {
                _logger.LogInformation(e.Message);
                return NotFound(e.Message);
            }
        }
        [Route("DeleteBankEmployee")]
        [HttpPut]
        public async Task<ActionResult<BankEmployees>> DeleteBank(int key)
        {
            try
            {
                return await _bankEmployeeService.DeleteBankEmployee(key);
            }
            catch (NoBankEmployeesFoundException e)
            {
                _logger.LogInformation(e.Message);
                return NotFound(e.Message);
            }
        }
    }

}

