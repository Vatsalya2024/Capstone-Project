﻿using System;
using Capstone_Project.Models;

namespace Capstone_Project.Interfaces
{
    public interface ICustomerAdminService : ICustomerUserService
    {
        public Task<List<Customers>> GetCustomersListasync();
        public Task<Customers> ChangeCustomerPhoneAsync(int id, long phone);
        public Task<Customers> ChangeCustomerName(int id, string name);
        public Task<Customers> ChangeCustomerAddress(int id, string address);
        public Task<Customers> DeleteCustomers(int id);
        Task<bool> UpdateCustomerPassword(string email, string newPassword);
    }
}

