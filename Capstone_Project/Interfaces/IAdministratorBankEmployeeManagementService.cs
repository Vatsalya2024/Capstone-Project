using System;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Interfaces
{
	public interface IAdministratorBankEmployeeManagementService
	{
        Task<BankEmployees?> ActivateEmployee(int employeeId);
        Task<BankEmployees?> DeactivateEmployee(int employeeId);
        Task<List<BankEmployees>?> GetAllEmployees();
        Task<BankEmployees?> GetEmployee(int employeeId);
        Task<BankEmployees?> CreateBankEmployee(RegisterBankEmployeeDTO employeeDTO);
        Task<BankEmployees?> UpdateEmployee(BankEmployees employee);

    }
}

