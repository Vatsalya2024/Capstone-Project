using System;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Capstone_Project.Interfaces
{
	public interface IBankEmployeeLoanService
	{
        Task<Loans> ReviewLoanApplication(int loanId);

        Task<bool> MakeLoanDecision(int loanId, bool approved);
        Task<Accounts> DisburseLoan(int loanId,long AccId);
        //Task<bool> ReviewAndApproveLoan(int loanId, int customerId);
        Task<ActionResult<CreditCheckResultDTO>> CheckCredit(long accountId);
    }
}

//LoanDisbursementDTO loanDisbursement