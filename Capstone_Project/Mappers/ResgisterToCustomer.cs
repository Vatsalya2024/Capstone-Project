using System;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Mappers
{
    public class RegisterToCustomer
    {
        Customers customer;
        public RegisterToCustomer(RegisterCustomerDTO register)
        {
            customer = new Customers();
            customer.Name = register.Name;
            customer.DOB = register.DOB;
            customer.AadharNumber = register.AadharNumber;
            customer.Address = register.Address;
            customer.Age = register.Age;
            customer.Gender = register.Gender;
            customer.PhoneNumber = register.PhoneNumber;
            customer.PANNumber = register.PANNumber;
            customer.Email = register.Email;
        }
        public Customers GetCustomers()
        {
            return customer;
        }
    }
}

