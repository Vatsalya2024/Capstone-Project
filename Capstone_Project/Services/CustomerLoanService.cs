using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Repositories;
using Capstone_Project.Services;

public class CustomerLoanService : ILoanCustomerService
{
    private readonly IRepository<int, Loans> _loansRepository;
    private readonly ILogger<CustomerLoanService> _logger;
    private readonly IRepository<int, Customers> _customerRepository;

    public CustomerLoanService(IRepository<int, Loans> loansRepository,IRepository<int,Customers> customerRepository, ILogger<CustomerLoanService> logger)
    {
        _loansRepository = loansRepository;
        _logger = logger;
        _customerRepository = customerRepository;
    }

  
    public async Task ApplyForLoan(LoanApplicationDTO loanApplication)
    {
        try
        {
            // Check if the customer is present
            if (loanApplication.CustomerID <= 0)
            {
                throw new NoCustomersFoundException("Customer ID not provided or invalid.");
            }

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
                Status = loanApplication.Status

            };

            await _loansRepository.Add(loan);
            _logger.LogInformation($"Loan application submitted for customer ID: {loanApplication.CustomerID}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error applying for loan: {ex.Message}");
            throw; // Re-throw the exception for handling in the controller
        }
    }

    

    public async Task<List<Loans>> ViewAvailedLoans(int customerId)
    {
        // Fetch the customer by ID
        var customer = await _customerRepository.Get(customerId);

        // Check if the customer exists
        if (customer == null)
        {
            throw new NoCustomersFoundException($"No customer found with ID {customerId}");
        }

        // Fetch all loans
        var allLoans = await _loansRepository.GetAll();

        // Filter loans based on customer ID
        var availedLoans = allLoans.Where(loan => loan.CustomerID == customerId).ToList();

        // Check if there are no loans for the customer
        if (availedLoans.Count == 0)
        {
            throw new NoLoansFoundException($"No loans found for customer with ID {customerId}");
        }

        return availedLoans;
    }

}


