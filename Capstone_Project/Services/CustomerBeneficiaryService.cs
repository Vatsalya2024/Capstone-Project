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

        
        public async Task AddBeneficiary(BeneficiaryDTO beneficiaryDTO)
        {
            try
            {
               
                string ifscCode = await GetIFSCByBranch(beneficiaryDTO.BranchName);

                if (!string.IsNullOrEmpty(ifscCode))
                {
                   
                    var customer = await _customerRepository.Get(beneficiaryDTO.CustomerId);

                    if (customer != null)
                    {
                       
                        var beneficiary = new Beneficiaries
                        {
                            AccountNumber = beneficiaryDTO.AccountNumber,
                            Name = beneficiaryDTO.Name,
                            IFSC = ifscCode,
                            CustomerID = customer.CustomerID, 
                                                             
                        };

                       
                        await _beneficiariesRepository.Add(beneficiary);

                        _logger.LogInformation("Beneficiary added successfully.");
                    }
                    else
                    {
                        _logger.LogWarning("Customer not found with the specified ID.");
                        throw new NoCustomersFoundException("No customer found with the specified ID.");
                    }
                }
                else
                {
                    _logger.LogWarning("IFSC code not found for the branch.");
                    throw new NoBranchesFoundException("IFSC code not found for the branch.");
                }
            }
            catch (NoCustomersFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while adding beneficiary: No customer found.");
                throw;
            }
            catch (NoBranchesFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while adding beneficiary: IFSC code not found for the branch.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding beneficiary.");
                throw; 
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
                if (bankBranch.Count == 0)
                {
                    throw new NoBanksFoundException("No branches found for the specified bank.");
                }

                _logger.LogInformation("Branches fetched successfully by bank.");
                return bankBranch;
              
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching branches by bank.");
                throw; 
            }
        }

        public async Task<string> GetIFSCByBranch(string branchName)
        {
            try
            {
                var branches = await _branchesRepository.GetAll();
                var branch = branches.FirstOrDefault(b => b.BranchName.Equals(branchName, StringComparison.OrdinalIgnoreCase));

                if (branch == null)
                {
                    throw new NoBranchesFoundException("Branch not found with the specified name.");
                }

                _logger.LogInformation("IFSC fetched successfully for the branch.");
                return branch.IFSCNumber;
            }
            catch (NoBranchesFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while fetching IFSC by branch: Branch not found.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching IFSC by branch.");
                throw; 
            }
        }
    }
}
