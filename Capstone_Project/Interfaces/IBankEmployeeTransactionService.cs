using System;
using Capstone_Project.Models;

namespace Capstone_Project.Interfaces
{
	public interface IBankEmployeeTransactionService
	{
        Task<List<Transactions>?> GetAllTransactions();
        Task<List<Transactions>?> GetTransactionsByAccountNumber(long accountNumber);
        Task<double> GetTotalInboundTransactions(long accountNumber);
        Task<double> GetTotalOutboundTransactions(long accountNumber);
    }
}

