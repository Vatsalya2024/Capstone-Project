using System;
using Capstone_Project.Models;

namespace Capstone_Project.Interfaces
{
	public interface IBeneficiaryService
	{
        
        Task<Beneficiaries> AddBeneficiaryAsync(Beneficiaries beneficiary);
    }
}

