using System;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Services
{
    public class BankEmployeeService : IBankEmployeeService
    {
        private readonly IRepository<int, BankEmployees> _bankEmployeeRepository;

        private readonly ILogger<BankEmployeeService> _logger;

        public BankEmployeeService(IRepository<int, BankEmployees> bankEmployeeRepository,

                    ILogger<BankEmployeeService> logger)
        {

            _bankEmployeeRepository = bankEmployeeRepository;

            _logger = logger;

        }




        public async Task<List<BankEmployees>> GetAllBankEmployee()
        {
            var allBankEmployee = await _bankEmployeeRepository.GetAll();
            if (allBankEmployee == null)
            {
                throw new NoBankEmployeesFoundException($"No Bank Employee Data Found");
            }
            _logger.LogInformation("All bank employees fetched successfully.");
            return allBankEmployee;
        }

        public async Task<BankEmployees> DeleteBankEmployee(int key)
        {
            var deletedBankEmployee = await _bankEmployeeRepository.Delete(key);
            if (deletedBankEmployee == null)
            {
                throw new NoBankEmployeesFoundException($"Bank Employee ID {key} not found");
            }
            _logger.LogInformation("Bank employees deleted successfully.");
            return deletedBankEmployee;
        }

        public async Task<BankEmployees> GetBankEmployee(int key)
        {
            var foundedBankEmployee = await _bankEmployeeRepository.Get(key);
            if (foundedBankEmployee == null)
            {
                throw new NoBankEmployeesFoundException($"Bank Employee ID {key} not found");
            }
            _logger.LogInformation("Bank employee fetched successfully.");
            return foundedBankEmployee;
        }

        public async Task<BankEmployees> UpdateBankEmployeeName(UpdateBankEmployeeNameDTO updateBankEmployeeNameDTO)
        {
            var foundedBankEmployee = await GetBankEmployee(updateBankEmployeeNameDTO.EmployeeID);
            foundedBankEmployee.Name = updateBankEmployeeNameDTO.Name;
            var updatedBankEmployee = await _bankEmployeeRepository.Update(foundedBankEmployee);
            _logger.LogInformation("Bank employees updated successfully.");
            return updatedBankEmployee;
        }
    }
}

