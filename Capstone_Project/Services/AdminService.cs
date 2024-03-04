using System;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Services
{
    public class AdminService : IAdminService
    {
        private readonly IRepository<int, Admin> _adminRepository;

        private readonly ILogger<AdminService> _logger;


        public AdminService(IRepository<int, Admin> adminRepository,


                     ILogger<AdminService> logger)
        {

            _adminRepository = adminRepository;

            _logger = logger;

        }


        public async Task<Admin> GetAdmin(int key)
        {
            var foundedAdmin = await _adminRepository.Get(key);
            if (foundedAdmin == null)
            {
                throw new NoAdminFoundException($"Admin ID {key} not found");
            }
            _logger.LogInformation($"Admin found.");
            return foundedAdmin;
        }

        public async Task<Admin> UpdateAdminName(UpdateBankAdminNameDTO updateAdminNameDTO)
        {
            var foundedAdmin = await GetAdmin(updateAdminNameDTO.AdminID);
            foundedAdmin.Name = updateAdminNameDTO.Name;
            var updatedAdmin = await _adminRepository.Update(foundedAdmin);
            return updatedAdmin;
        }

        public async Task<List<Admin>> GetAllAdmin()
        {
            var allAdmin = await _adminRepository.GetAll();
            if (allAdmin == null)
            {
                throw new NoAdminFoundException($"No Admin Data Found");
            }
            _logger.LogInformation($"Fetched all admins.");
            return allAdmin;
        }

        public async Task<Admin> DeleteAdmin(int key)
        {
            var deletedAdmin = await _adminRepository.Delete(key);
            if (deletedAdmin == null)
            {
                throw new NoAdminFoundException($"Admin ID {key} not found");
            }
            _logger.LogInformation($"Deleted admin.");
            return deletedAdmin;
        }
    }
}

