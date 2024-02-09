using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Mappers;

namespace Capstone_Project.Services
{
    public class AdministratorBankEmployeeManagementService : IAdministratorBankEmployeeManagementService
    {
        private readonly IRepository<int, BankEmployees> _bankEmployeesRepository;
        private readonly IRepository<string, Validation> _validationRepository;
        private readonly ILogger<AdministratorBankEmployeeManagementService> _logger;

        public AdministratorBankEmployeeManagementService(IRepository<int, BankEmployees> bankEmployeesRepository,
            IRepository<string, Validation> validationRepository,
            ILogger<AdministratorBankEmployeeManagementService> logger)
        {
            _bankEmployeesRepository = bankEmployeesRepository;
            _validationRepository = validationRepository;
            _logger = logger;
        }

        public async Task<BankEmployees?> ActivateEmployee(int employeeId)
        {
            var employee = await _bankEmployeesRepository.Get(employeeId);
            if (employee != null)
            {
                var validation = await _validationRepository.Get(employee.Email);
                if (validation != null)
                {
                    validation.Status = "Active";
                    await _validationRepository.Update(validation);
                    return employee;
                }
            }
            return null;
        }

        public async Task<BankEmployees?> DeactivateEmployee(int employeeId)
        {
            var employee = await _bankEmployeesRepository.Get(employeeId);
            if (employee != null)
            {
                var validation = await _validationRepository.Get(employee.Email);
                if (validation != null)
                {
                    validation.Status = "deactivated";
                    await _validationRepository.Update(validation);

                    _logger.LogInformation($"Employee with ID {employeeId} deactivated.");
                    return employee;
                }
            }
            return null;
        }

        public async Task<List<BankEmployees>?> GetAllEmployees()
        {
            var employees = await _validationRepository.GetAll();
            if (employees != null)
            {
                var filteredEmployees = employees
                    .Where(u => u.UserType == "BankEmployee")
                    .Select(u => u.Email)
                    .ToList();

                var bankEmployees = new List<BankEmployees>();
                foreach (var email in filteredEmployees)
                {
                    var employeeList = await _bankEmployeesRepository.GetAll();
                    var employee = employeeList
                        .Where(e => e.Email == email)
                        .FirstOrDefault();

                    if (employee != null)
                    {
                        bankEmployees.Add(employee);
                    }
                }

                return bankEmployees;
            }

            return null;
        }

        public async Task<BankEmployees?> GetEmployee(int employeeId)
        {
            var employee = await _bankEmployeesRepository.Get(employeeId);
            return employee;
        }

        public async Task<BankEmployees?> CreateBankEmployee(RegisterBankEmployeeDTO employeeDTO)
        {
            try
            {
                // Map RegisterBankEmployeeDTO to BankEmployees entity
                var newBankEmployee = new RegiterToBankEmployee(employeeDTO).GetBankEmployees();

                // Map RegisterBankEmployeeDTO to Validation entity
                var newValidation = new RegisterToBankEmployeeUser(employeeDTO).GetValidation();

                // Add the new validation to the database
                var addedValidation = await _validationRepository.Add(newValidation);

                // Link the bank employee entity to the validation entity
                newBankEmployee.Email = addedValidation.Email;

                // Add the new bank employee to the database
                var addedBankEmployee = await _bankEmployeesRepository.Add(newBankEmployee);

                return addedBankEmployee;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating bank employee: {ex.Message}");
                return null;
            }
        }
        public async Task<BankEmployees?> UpdateEmployee(BankEmployees employee)
        {
            try
            {
                // Update the employee in the repository
                var updatedEmployee = await _bankEmployeesRepository.Update(employee);
                return updatedEmployee;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating employee: {ex.Message}");
                return null;
            }
        }
    }
}
