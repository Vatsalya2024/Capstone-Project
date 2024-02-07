using System;
using Capstone_Project.Context;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Capstone_Project.Repositories
{
    public class BankEmployeesRepository : IRepository<int, BankEmployees>
    {
        private readonly MavericksBankContext _mavericksBankContext;
        private readonly ILogger<BankEmployeesRepository> _loggerBankEmployeesRepository;

        public BankEmployeesRepository(MavericksBankContext mavericksBankContext, ILogger<BankEmployeesRepository> loggerBankEmployeesRepository)
        {
            _mavericksBankContext = mavericksBankContext;
            _loggerBankEmployeesRepository = loggerBankEmployeesRepository;
        }

        public async Task<BankEmployees> Add(BankEmployees item)
        {
            _mavericksBankContext.BankEmployees.Add(item);
            await _mavericksBankContext.SaveChangesAsync();
            _loggerBankEmployeesRepository.LogInformation($"Added New Employee : {item.EmployeeID}");
            return item;
        }

        public async Task<BankEmployees?> Delete(int key)
        {
            var foundedEmployee = await Get(key);
            if (foundedEmployee == null)
            {
                return null;
            }
            else
            {
                _mavericksBankContext.BankEmployees.Remove(foundedEmployee);
                await _mavericksBankContext.SaveChangesAsync();
                return foundedEmployee;
            }
        }

        public async Task<BankEmployees?> Get(int key)
        {
            var foundedEmployee = await _mavericksBankContext.BankEmployees.FirstOrDefaultAsync(employee => employee.EmployeeID == key);
            if (foundedEmployee == null)
            {
                return null;
            }
            else
            {
                return foundedEmployee;
            }
        }

        public async Task<List<BankEmployees>?> GetAll()
        {
            var allEmployees = await _mavericksBankContext.BankEmployees.ToListAsync();
            if (allEmployees.Count == 0)
            {
                return null;
            }
            else
            {
                return allEmployees;
            }
        }

        public async Task<BankEmployees> Update(BankEmployees item)
        {
            _mavericksBankContext.Entry<BankEmployees>(item).State = EntityState.Modified;
            await _mavericksBankContext.SaveChangesAsync();
            return item;
        }
    }
}

