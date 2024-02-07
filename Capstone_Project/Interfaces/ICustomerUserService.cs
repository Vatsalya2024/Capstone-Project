using System;
using Capstone_Project.Models;

namespace Capstone_Project.Interfaces
{
    public interface ICustomerUserService
    {
        public Task<Customers> GetCustomers(int id);
    }
}

