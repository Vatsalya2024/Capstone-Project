using System;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Interfaces
{
	public interface ICustomerBeneficiaryService
	{
        //Task<bool> AddBeneficiary(BeneficiaryDTO beneficiaryDTO);
        Task<Beneficiaries> AddBeneficiary(BeneficiaryDTO beneficiaryDTO);
        //Task<string> TransferToBeneficiary(long sourceAccountNumber, long beneficiaryAccountNumber, double amount);
        Task<string> TransferToBeneficiary(BeneficiaryTransferDTO transferDTO);
        Task<List<BranchDTO>> GetBranchesByBank(string bankName);
        Task<string> GetIFSCByBranch(string branchName);
        Task<List<Beneficiaries>> GetBeneficiariesByCustomerID(int customerID);
    }
}

