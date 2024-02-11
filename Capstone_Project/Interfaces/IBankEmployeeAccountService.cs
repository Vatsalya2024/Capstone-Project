using System;
using Capstone_Project.Models;

namespace Capstone_Project.Interfaces
{
	public interface IBankEmployeeAccountService
	{
        Task<bool> ApproveAccountCreation(long accountNumber);
        Task<bool> ApproveAccountDeletion(long accountNumber);
        Task<List<Accounts>> GetPendingAccounts();
        Task<List<Accounts>> GetPendingDeletionAccounts();
        Task<List<Customers>> GetCustomersListasync();
        Task<Customers> GetCustomers(int id);
    }
}

