using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Microsoft.EntityFrameworkCore;
using Capstone_Project.Context;
using Microsoft.Extensions.Logging;

namespace Capstone_Project.Repositories
{
    public class AvailableLoansRepository : IRepository<int, AvailableLoans>
    {
        private readonly MavericksBankContext _context;
        private readonly ILogger<AvailableLoansRepository> _logger;

        public AvailableLoansRepository(MavericksBankContext context, ILogger<AvailableLoansRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<AvailableLoans> Add(AvailableLoans item)
        {
            _context.AvailableLoans.Add(item);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Added a new loan.");
            return item;
        }

        public async Task<AvailableLoans?> Delete(int key)
        {
            var loan = await _context.AvailableLoans.FindAsync(key);
            if (loan != null)
            {
                _context.AvailableLoans.Remove(loan);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Deleted the loan with key {key}.");
            }
            return loan;
        }

        public async Task<AvailableLoans?> Get(int key)
        {
            _logger.LogInformation($"Retrieving the loan with key {key}.");
            return await _context.AvailableLoans.FindAsync(key);
        }

        public async Task<List<AvailableLoans>?> GetAll()
        {
            _logger.LogInformation("Retrieving all loans.");
            return await _context.AvailableLoans.ToListAsync();
        }

        public async Task<AvailableLoans> Update(AvailableLoans item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Updated the loan.");
            return item;
        }
    }
}
