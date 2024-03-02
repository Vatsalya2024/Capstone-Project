using System.Threading.Tasks;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Microsoft.Extensions.Logging;

namespace Capstone_Project.Services
{
    public class AdminAvailableLoansService : IAdminAvailableLoansService
    {
        private readonly IRepository<int, AvailableLoans> _availableLoansRepository;
        private readonly ILogger<AdminAvailableLoansService> _logger;

        public AdminAvailableLoansService(
            IRepository<int, AvailableLoans> availableLoansRepository,
            ILogger<AdminAvailableLoansService> logger)
        {
            _availableLoansRepository = availableLoansRepository;
            _logger = logger;
        }

        public async Task<AvailableLoans> AddLoan(AvailableLoans loan)
        {
            _logger.LogInformation("Adding a new loan.");
            return await _availableLoansRepository.Add(loan);
        }
    }
}
