using System;
using Capstone_Project.Models;

namespace Capstone_Project.Interfaces
{
	public interface IAvailableLoansUserService
	{
        Task<List<AvailableLoans>?> GetAllLoans();
    }
}

