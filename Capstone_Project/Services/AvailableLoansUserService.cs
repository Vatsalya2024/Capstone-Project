using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Microsoft.Extensions.Logging;

namespace Capstone_Project.Services
{
    public class AvailableLoansUserService : IAvailableLoansUserService
    {
        private readonly IRepository<int, AvailableLoans> _availableLoansRepository;
        private readonly ILogger<AvailableLoansUserService> _logger;

        public AvailableLoansUserService(IRepository<int, AvailableLoans> availableLoansRepository, ILogger<AvailableLoansUserService> logger)
        {
            _availableLoansRepository = availableLoansRepository;
            _logger = logger;
        }

        public async Task<List<AvailableLoans>?> GetAllLoans()
        {
            try
            {
                _logger.LogInformation("Retrieving all loans for user.");
                return await _availableLoansRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all loans for user.");
                throw;
            }
        }
    }
}
