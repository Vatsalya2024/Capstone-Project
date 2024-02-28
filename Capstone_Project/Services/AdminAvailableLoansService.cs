using System.Threading.Tasks;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;

namespace Capstone_Project.Services
{
    public class AdminAvailableLoansService : IAdminAvailableLoansService
    {
        private readonly IRepository<int, AvailableLoans> _availableLoansRepository;

        public AdminAvailableLoansService(IRepository<int, AvailableLoans> availableLoansRepository)
        {
            _availableLoansRepository = availableLoansRepository;
        }

        public async Task<AvailableLoans> AddLoan(AvailableLoans loan)
        {
            return await _availableLoansRepository.Add(loan);
        }

        
    }
}
