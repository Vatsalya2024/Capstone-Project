using System;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Services;
using Microsoft.AspNetCore.Mvc;

namespace Capstone_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AdminController : ControllerBase
    {

        private readonly ILogger<AdminController> _logger;
        private readonly IAdminService _adminService;

        public AdminController(ILogger<AdminController> logger, IAdminService adminService)
        {

            _logger = logger;
            _adminService = adminService;
        }

        [Route("GetAllAdmin")]
        [HttpGet]
        public async Task<ActionResult<List<Admin>>> GetAllAdmin()
        {
            try
            {
                _logger.LogInformation("Retrieved Admin successfully.");
                return await _adminService.GetAllAdmin();
            }
            catch (NoAdminFoundException e)
            {
                _logger.LogInformation(e.Message);
                return NotFound(e.Message);
            }
        }

        [Route("GetAdmin")]
        [HttpGet]
        public async Task<ActionResult<Admin>> GetAdmin(int key)
        {
            try
            {
                _logger.LogInformation("Retrieved Admin successfully.");
                return await _adminService.GetAdmin(key);
            }
            catch (NoAdminFoundException e)
            {
                _logger.LogInformation(e.Message);
                return NotFound(e.Message);
            }
        }

        [Route("UpdateAdminName")]
        [HttpPut]
        public async Task<ActionResult<Admin>> UpdateAdminName(UpdateBankAdminNameDTO updateAdminNameDTO)
        {
            try
            {
                return await _adminService.UpdateAdminName(updateAdminNameDTO);
            }
            catch (NoAdminFoundException e)
            {
                _logger.LogInformation(e.Message);
                return NotFound(e.Message);
            }
        }
        [Route("DeleteAdmin")]
        [HttpPut]
        public async Task<ActionResult<Admin>> DeleteAdmin(int key)
        {
            try
            {
                return await _adminService.DeleteAdmin(key);
            }
            catch (NoAdminFoundException e)
            {
                _logger.LogInformation(e.Message);
                return NotFound(e.Message);
            }
        }




    }
}

