using System;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Interfaces
{
	public interface ICustomerBeneficiaryService
	{
        //Task<bool> AddBeneficiary(BeneficiaryDTO beneficiaryDTO);
        Task AddBeneficiary(BeneficiaryDTO beneficiaryDTO);
        Task<List<BranchDTO>> GetBranchesByBank(string bankName);
        Task<string> GetIFSCByBranch(string branchName);
    }
}

