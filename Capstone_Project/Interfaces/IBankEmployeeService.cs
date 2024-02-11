using System;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Interfaces
{
    public interface IBankEmployeeService
    {
        public Task<List<BankEmployees>> GetAllBankEmployee();
        public Task<BankEmployees> DeleteBankEmployee(int key);

        public Task<BankEmployees> GetBankEmployee(int key);
        public Task<BankEmployees> UpdateBankEmployeeName(UpdateBankEmployeeNameDTO updateBankEmployeeNameDTO);
    }

}

