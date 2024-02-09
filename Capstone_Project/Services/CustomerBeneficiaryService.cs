using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Capstone_Project.Services
{
    public class CustomerBeneficiaryService : ICustomerBeneficiaryService
    {
        private readonly IRepository<int, Beneficiaries> _beneficiariesRepository;
        private readonly IRepository<string, Branches> _branchesRepository;
        private readonly ILogger<CustomerBeneficiaryService> _logger;
        private readonly IRepository<int, Customers> _customerRepository;

        public CustomerBeneficiaryService(
            IRepository<int, Beneficiaries> beneficiariesRepository,
            IRepository<string, Branches> branchesRepository, IRepository<int, Customers> customerRepository,
            ILogger<CustomerBeneficiaryService> logger)
        {
            _beneficiariesRepository = beneficiariesRepository;
            _branchesRepository = branchesRepository;
            _logger = logger;
            _customerRepository = customerRepository;
        }

        //public async Task<bool> AddBeneficiary(BeneficiaryDTO beneficiaryDTO)
        //{
        //    try
        //    {
        //        // Fetch the IFSC code based on the branch name
        //        string ifscCode = await GetIFSCByBranch(beneficiaryDTO.BranchName);

        //        if (!string.IsNullOrEmpty(ifscCode))
        //        {
        //            // Create Beneficiary entity
        //            var beneficiary = new Beneficiaries
        //            {
        //                AccountNumber = beneficiaryDTO.AccountNumber,
        //                Name = beneficiaryDTO.Name,
        //                IFSC = ifscCode,
        //                // Assign other properties accordingly
        //            };

        //            // Add the beneficiary to the repository
        //            await _beneficiariesRepository.Add(beneficiary);

        //            _logger.LogInformation("Beneficiary added successfully.");
        //            return true;
        //        }
        //        else
        //        {
        //            _logger.LogWarning("IFSC code not found for the branch.");
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while adding beneficiary.");
        //        throw; // Re-throwing the exception for higher-level handling
        //    }
        //}

        public async Task<bool> AddBeneficiary(BeneficiaryDTO beneficiaryDTO)
        {
            try
            {
                // Fetch the IFSC code based on the branch name
                string ifscCode = await GetIFSCByBranch(beneficiaryDTO.BranchName);

                if (!string.IsNullOrEmpty(ifscCode))
                {
                    // Fetch customer by ID (you should implement this method)
                    var customer = await _customerRepository.Get(beneficiaryDTO.CustomerId);

                    if (customer != null)
                    {
                        // Create Beneficiary entity
                        var beneficiary = new Beneficiaries
                        {
                            AccountNumber = beneficiaryDTO.AccountNumber,
                            Name = beneficiaryDTO.Name,
                            IFSC = ifscCode,
                            CustomerID = customer.CustomerID, // Assign CustomerID
                                                              // Assign other properties accordingly
                        };

                        // Add the beneficiary to the repository
                        await _beneficiariesRepository.Add(beneficiary);

                        _logger.LogInformation("Beneficiary added successfully.");
                        return true;
                    }
                    else
                    {
                        _logger.LogWarning("Customer not found with the specified ID.");
                        return false;
                    }
                }
                else
                {
                    _logger.LogWarning("IFSC code not found for the branch.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding beneficiary.");
                throw; // Re-throwing the exception for higher-level handling
            }
        }



        public async Task<List<BranchDTO>> GetBranchesByBank(string bankName)
        {
            try
            {
                var branches = await _branchesRepository.GetAll();
                var bankBranch = branches
                    .Where(bankBranch => bankBranch.Banks.BankName == bankName)
                    .Select(bankBranch => new BranchDTO
                    {
                        BranchName = bankBranch.BranchName,
                        IFSC = bankBranch.IFSCNumber
                    })
                    .ToList();

                _logger.LogInformation("Branches fetched successfully by bank.");
                return bankBranch;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching branches by bank.");
                throw; // Re-throwing the exception for higher-level handling
            }
        }

        public async Task<string> GetIFSCByBranch(string branchName)
        {
            try
            {
                var branches = await _branchesRepository.GetAll();
                var branch = branches.FirstOrDefault(b => b.BranchName.Equals(branchName, StringComparison.OrdinalIgnoreCase));

                _logger.LogInformation("IFSC fetched successfully for the branch.");
                return branch?.IFSCNumber;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching IFSC by branch.");
                throw; // Re-throwing the exception for higher-level handling
            }
        }
    }
}
