using System;
using Capstone_Project.Models;

namespace Capstone_Project.Interfaces
{
	public interface IAdminAvailableLoansService
	{
        Task<AvailableLoans> AddLoan(AvailableLoans loan);
    }
}

