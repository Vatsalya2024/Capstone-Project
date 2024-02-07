using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Capstone_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchesController : ControllerBase
    {
        private readonly IBranchesAdminService _branchesService;
        private readonly ILogger<BranchesController> _loggerBranchesController;

        public BranchesController(IBranchesAdminService branchesService, ILogger<BranchesController> loggerBranchesController)
        {
            _branchesService = branchesService;
            _loggerBranchesController = loggerBranchesController;
        }

        [Route("GetAllBranches")]
        [HttpGet]
        public async Task<ActionResult<List<Branches>>> GetAllBranches()
        {
            try
            {
                return await _branchesService.GetAllBranches();
            }
            catch (NoBranchesFoundException e)
            {
                _loggerBranchesController.LogInformation(e.Message);
                return NotFound(e.Message);
            }
        }

        [Route("GetBranch")]
        [HttpGet]
        public async Task<ActionResult<Branches>> GetBranch(string key)
        {
            try
            {
                return await _branchesService.GetBranch(key);
            }
            catch (NoBranchesFoundException e)
            {
                _loggerBranchesController.LogInformation(e.Message);
                return NotFound(e.Message);
            }
        }

        [Route("AddBranch")]
        [HttpPost]
        public async Task<ActionResult<Branches>> AddBranch(Branches item)
        {
            return await _branchesService.AddBranch(item);
        }

        [Route("UpdateBranchName")]
        [HttpPut]
        public async Task<ActionResult<Branches>> UpdateBranchName(UpdateBranchNameDTO updateBranchNameDTO)
        {
            try
            {
                return await _branchesService.UpdateBranchName(updateBranchNameDTO);
            }
            catch (NoBranchesFoundException e)
            {
                _loggerBranchesController.LogInformation(e.Message);
                return NotFound(e.Message);
            }
        }

        [Route("DeleteBranch")]
        [HttpPut]
        public async Task<ActionResult<Branches>> DeleteBranch(string key)
        {
            try
            {
                return await _branchesService.DeleteBranch(key);
            }
            catch (NoBranchesFoundException e)
            {
                _loggerBranchesController.LogInformation(e.Message);
                return NotFound(e.Message);
            }
        }
    }
}

