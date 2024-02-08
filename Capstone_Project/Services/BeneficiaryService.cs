using Capstone_Project.Exceptions;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Capstone_Project.Services
{
    public class BeneficiaryService : IBeneficiaryService
    {
        
        private readonly IRepository<int, Beneficiaries> _beneficiariesRepository;
        private readonly ILogger<BeneficiaryService> _logger;

        public BeneficiaryService( IRepository<int, Beneficiaries> beneficiariesRepository, ILogger<BeneficiaryService> logger)
        {
           
            _beneficiariesRepository = beneficiariesRepository;
            _logger = logger;
        }

        

        public async Task<Beneficiaries> AddBeneficiaryAsync(Beneficiaries beneficiary)
        {
            try
            {
                return await _beneficiariesRepository.Add(beneficiary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding beneficiary");
                throw new BeneficiaryServiceException("Error occurred while adding beneficiary", ex);
            }
        }
    }
}
