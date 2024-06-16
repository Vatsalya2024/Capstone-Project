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
            
            if (loanApplication.CustomerID <= 0)
            {
                throw new NoCustomersFoundException("Customer ID not provided or invalid.");
            }

           
            loanApplication.Status = "Pending";

          
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
            throw; 
        }
    }

    

    public async Task<List<Loans>> ViewAvailedLoans(int customerId)
    {
        var customer = await _customerRepository.Get(customerId);

       
        if (customer == null)
        {
            throw new NoCustomersFoundException($"No customer found with ID {customerId}");
        }

 
        var allLoans = await _loansRepository.GetAll();
        if (allLoans == null)
        {
            throw new NoLoansFoundException("No loans found");
        }

        
        var availedLoans = allLoans.Where(loan => loan.CustomerID == customerId).ToList();

        
        if (availedLoans.Count == 0)
        {
            throw new NoLoansFoundException($"No loans found for customer with ID {customerId}");
        }

        return availedLoans;
    }


}


