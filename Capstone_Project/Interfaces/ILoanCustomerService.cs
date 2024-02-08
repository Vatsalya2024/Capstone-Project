using System;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Interfaces
{
	public interface ILoanCustomerService
	{
        Task<bool> ApplyForLoan(LoanApplicationDTO loanApplication);
        Task<List<Loans>> ViewAvailedLoans();
    }
}

