using System;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;

namespace Capstone_Project.Services
{
	public class AvailableLoansUserService: IAvailableLoansUserService
    {
        private readonly IRepository<int, AvailableLoans> _availableLoansRepository;

        public AvailableLoansUserService(IRepository<int, AvailableLoans> availableLoansRepository)
        {
            _availableLoansRepository = availableLoansRepository;
        }

        public async Task<List<AvailableLoans>?> GetAllLoans()
        {
            return await _availableLoansRepository.GetAll();
        }

        
    }
}


