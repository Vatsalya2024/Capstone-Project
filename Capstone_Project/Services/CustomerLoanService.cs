using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;

public class CustomerLoanService : ILoanCustomerService
{
    private readonly IRepository<int, Loans> _loansRepository;
    private readonly ILogger<CustomerLoanService> _logger;

    public CustomerLoanService(IRepository<int, Loans> loansRepository, ILogger<CustomerLoanService> logger)
    {
        _loansRepository = loansRepository;
        _logger = logger;
    }

    public async Task<bool> ApplyForLoan(LoanApplicationDTO loanApplication)
    {
        try
        {
            // Set status to "Pending" when applying for a loan
            loanApplication.Status = "Pending";

            // Convert LoanApplication to Loans entity
            var loan = new Loans
            {
                LoanAmount = loanApplication.LoanAmount,
                LoanType = loanApplication.LoanType,
                Interest = loanApplication.Interest,
                Tenure = loanApplication.Tenure,
                Purpose = loanApplication.Purpose,
                CustomerID = loanApplication.CustomerID,
                Status = loanApplication.Status // Set status here
            };

            await _loansRepository.Add(loan);
            _logger.LogInformation($"Loan application submitted for customer ID: {loanApplication.CustomerID}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error applying for loan: {ex.Message}");
            return false;
        }
    }

    public async Task<List<Loans>> ViewAvailedLoans()
    {
        try
        {
            var availedLoans = await _loansRepository.GetAll();
            return availedLoans ?? new List<Loans>();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error viewing availed loans: {ex.Message}");
            return new List<Loans>();
        }
    }

    // You can add a method for bank employees to update the status of the loan application
    //public async Task<bool> UpdateLoanStatus(int loanId, string newStatus)
    //{
    //    try
    //    {
    //        var loan = await _loansRepository.Get(loanId);
    //        if (loan != null)
    //        {
    //            loan.Status = newStatus;
    //            await _loansRepository.Update(loan);
    //            _logger.LogInformation($"Loan status updated for Loan ID: {loanId} to {newStatus}");
    //            return true;
    //        }
    //        else
    //        {
    //            _logger.LogError($"Loan ID {loanId} not found.");
    //            return false;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError($"Error updating loan status: {ex.Message}");
    //        return false;
    //    }
    //}
}
