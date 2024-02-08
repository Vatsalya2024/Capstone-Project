using System;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Interfaces
{
	public interface IBankEmployeeLoanService
	{
        Task<bool> ReviewLoanApplication(int loanId);

        Task<bool> MakeLoanDecision(int loanId, bool approved);
        //Task<bool> DisburseLoan(int loanId);
        //Task<bool> ReviewAndApproveLoan(int loanId, int customerId);
    }
}

//LoanDisbursementDTO loanDisbursement