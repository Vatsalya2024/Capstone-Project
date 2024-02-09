using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Mappers;

namespace Capstone_Project.Services
{
    public class AdministratorCustomerManagementService : IAdministratorCustomerManagementService
    {
        private readonly IRepository<int, Customers> _customersRepository;
        private readonly IRepository<string, Validation> _validationRepository;
        private readonly ILogger<AdministratorCustomerManagementService> _logger;

        public AdministratorCustomerManagementService(IRepository<int, Customers> customersRepository,
            IRepository<string, Validation> validationRepository,
            ILogger<AdministratorCustomerManagementService> logger)
        {
            _customersRepository = customersRepository;
            _validationRepository = validationRepository;
            _logger = logger;
        }

        public async Task<Customers?> ActivateUser(int customerId)
        {
            var user = await _customersRepository.Get(customerId);
            if (user != null)
            {
                var validation = await _validationRepository.Get(user.Email);
                if (validation != null)
                {
                    validation.Status = "Active";
                    await _validationRepository.Update(validation);
                    return user;
                }
            }
            return null;
        }

        public async Task<Customers?> DeactivateUser(int customerId)
        {
            var user = await _customersRepository.Get(customerId);
            if (user != null)
            {
                var validation = await _validationRepository.Get(user.Email);
                if (validation != null)
                {
                    validation.Status = "deactivated";
                    await _validationRepository.Update(validation);

                    _logger.LogInformation($"User with ID {customerId} deactivated.");
                    return user;
                }
            }
            return null;
        }

        public async Task<List<Customers>?> GetAllUsers()
        {
            var users = await _validationRepository.GetAll();
            if (users != null)
            {
                var filteredUsers = users
                    .Where(u => u.UserType == "Customer")
                    .Select(u => u.Email)
                    .ToList();

                var customers = new List<Customers>();
                foreach (var email in filteredUsers)
                {
                    var customerList = await _customersRepository.GetAll();
                    var customer = customerList
                        .Where(c => c.Email == email)
                        .FirstOrDefault();

                    if (customer != null)
                    {
                        customers.Add(customer);
                    }
                }

                return customers;
            }

            return null;
        }

        public async Task<Customers?> GetUser(int customerId)
        {
            var customer = await _customersRepository.Get(customerId);
            return customer;
        }

        public async Task<Customers?> UpdateCustomerContact(int customerId, AdminUpdateCustomerContactDTO contactDTO)
        {
            var customer = await _customersRepository.Get(customerId);
            if (customer != null)
            {
                AdminCustomerMapper.MapToCustomer(contactDTO, customer);
                await _customersRepository.Update(customer);
                return customer;
            }
            return null;
        }

        public async Task<Customers?> UpdateCustomerDetails(int customerId, AdminUpdateCustomerDetailsDTO detailsDTO)
        {
            var customer = await _customersRepository.Get(customerId);
            if (customer != null)
            {
                AdminCustomerMapper.MapToCustomer(detailsDTO, customer);
                await _customersRepository.Update(customer);
                return customer;
            }
            return null;
        }

        public async Task<Customers?> UpdateCustomerName(int customerId, AdminUpdateCustomerNameDTO nameDTO)
        {
            var customer = await _customersRepository.Get(customerId);
            if (customer != null)
            {
                AdminCustomerMapper.MapToCustomer(nameDTO, customer);
                await _customersRepository.Update(customer);
                return customer;
            }
            return null;
        }
        public async Task<Customers?> CreateCustomer(RegisterCustomerDTO customerDTO)
        {
            try
            {
                // Map RegisterCustomerDTO to Customers entity
                var newCustomer = new RegisterToCustomer(customerDTO).GetCustomers();

                // Map RegisterCustomerDTO to Validation entity
                var newValidation = new RegisterToCustomerUser(customerDTO).GetValidation();

                // Add the new validation to the database
                var addedValidation = await _validationRepository.Add(newValidation);

                // Link the customer entity to the validation entity
                newCustomer.Email = addedValidation.Email;

                // Add the new customer to the database
                var addedCustomer = await _customersRepository.Add(newCustomer);

                return addedCustomer;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating customer: {ex.Message}");
                return null;
            }
        }

    }

}

