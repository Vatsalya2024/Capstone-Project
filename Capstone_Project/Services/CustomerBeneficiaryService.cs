using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone_Project.Controllers;
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
        private readonly IRepository<int, Beneficiaries> _beneficiaryRepository;
        private readonly IRepository<string, Branches> _branchesRepository;
        private readonly ILogger<CustomerBeneficiaryService> _logger;
        private readonly IRepository<int, Customers> _customerRepository;
        private readonly IRepository<int, Transactions> _transactionRepository;
        private readonly IRepository<long, Accounts> _accountRepository;

        public CustomerBeneficiaryService(
            IRepository<int, Beneficiaries> beneficiaryRepository,
            IRepository<string, Branches> branchesRepository, IRepository<int, Customers> customerRepository, IRepository<long, Accounts> accountRepository,
            IRepository<int, Transactions> transactionRepository,
            ILogger<CustomerBeneficiaryService> logger)
        {
            _beneficiaryRepository = beneficiaryRepository;
            _branchesRepository = branchesRepository;
            _logger = logger;
            _customerRepository = customerRepository;
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
        }

        public async Task<List<Beneficiaries>> GetBeneficiariesByCustomerID(int customerID)
        {
            
                var beneficiaries = await _beneficiaryRepository.GetAll();
                if (beneficiaries == null)
                {
                    throw new NoBeneficiariesFoundException("No Beneficiary found");
                }
                var customerBeneficiaries = beneficiaries.Where(b => b.CustomerID == customerID).ToList();

                if (customerBeneficiaries.Count == 0)
                {
                    _logger.LogInformation($"No beneficiaries found for customer with ID: {customerID}");
                    throw new NoCustomersFoundException($"No beneficiaries found for customer with ID: {customerID}");
                }

                return customerBeneficiaries;
           
        }


        public async Task<Beneficiaries> AddBeneficiary(BeneficiaryDTO beneficiaryDTO)
        {
            
            Beneficiaries beneficiary = new Beneficiaries
            {
                BeneficiaryAccountNumber = beneficiaryDTO.BeneficiaryAccountNumber,
                Name = beneficiaryDTO.Name,
                IFSC = beneficiaryDTO.IFSC,
                CustomerID = beneficiaryDTO.CustomerID,
                Balance = 0 
            };

           
            return await _beneficiaryRepository.Add(beneficiary);
        }





        public async Task<List<BranchDTO>> GetBranchesByBank(string bankName)
        {
            try
            {
                var branches = await _branchesRepository.GetAll();
                if (branches == null)
                {
                    throw new NoBranchesFoundException("No branches found");
                }
                if (bankName.Equals("MB", StringComparison.OrdinalIgnoreCase))
                {
                    return new List<BranchDTO>();
                }

                var bankBranch = branches
             .Where(bankBranch => bankBranch.Banks != null && bankBranch.Banks.BankName != null &&
                                   bankBranch.Banks.BankName.Equals(bankName, StringComparison.OrdinalIgnoreCase))
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
                if (branches == null)
                {
                    throw new NoBranchesFoundException("Branch not found.");
                }
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



        public async Task<string> TransferToBeneficiary(BeneficiaryTransferDTO transferDTO)
        {
            var sourceAccount = await _accountRepository.Get(transferDTO.SourceAccountNumber);
            var beneficiaryID = transferDTO.BeneficiaryID;

            var beneficiary = await _beneficiaryRepository.Get(beneficiaryID);

            if (sourceAccount == null || beneficiary == null )
            {
                throw new NoAccountsFoundException("Account Not Found");
            }

            if (sourceAccount.Status != "Active")
            {
                throw new NoAccountsFoundException("Account not active");
            }

           var beneficiaryAccountNumber = beneficiary.BeneficiaryAccountNumber;

            if (sourceAccount.Balance < transferDTO.Amount)
            {
                throw new NotSufficientBalanceException();
            }

            sourceAccount.Balance -= transferDTO.Amount;
            beneficiary.Balance += transferDTO.Amount;

            await _accountRepository.Update(sourceAccount);
            await _beneficiaryRepository.Update(beneficiary);

            var transaction = new Transactions
            {
                Amount = transferDTO.Amount,
                TransactionDate = DateTime.Now,
                Description = $"Transfer to Beneficiary:{beneficiary.Name},{beneficiaryAccountNumber}",
                TransactionType = "Debit",
                Status = "Completed",
                SourceAccountNumber = transferDTO.SourceAccountNumber,
                BeneficiaryID = transferDTO.BeneficiaryID 
            };

            await _transactionRepository.Add(transaction);

            return $"Transfer of {transferDTO.Amount} rupees from account {transferDTO.SourceAccountNumber} to account {beneficiaryAccountNumber} was successful.";
        }




    }
}





