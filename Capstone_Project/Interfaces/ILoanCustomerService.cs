using System;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Interfaces
{
	public interface ILoanCustomerService
	{
        Task ApplyForLoan(LoanApplicationDTO loanApplication);
        //Task<List<Loans>> ViewAvailedLoans();
        Task<List<Loans>> ViewAvailedLoans(int customerId);
    }
}

