using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Mappers;
using System.Runtime.Serialization;

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
            try
            {
                var user = await _customersRepository.Get(customerId);
                if (user == null)
                {
                    throw new NoCustomersFoundException($"User with ID {customerId} not found.");
                }

                var validation = await _validationRepository.Get(user.Email);
                if (validation == null)
                {
                    throw new ValidationNotFoundException($"Validation for user with ID {customerId} not found.");
                }

                validation.Status = "Active";
                await _validationRepository.Update(validation);
                _logger.LogInformation($"User with ID {customerId} activated.");
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error activating user: {ex.Message}");
                throw;
            }
        }


        public async Task<Customers?> DeactivateUser(int customerId)
        {
            try
            {
                var user = await _customersRepository.Get(customerId);
                if (user == null)
                {
                    throw new NoCustomersFoundException($"User with ID {customerId} not found.");
                }

                var validation = await _validationRepository.Get(user.Email);
                if (validation == null)
                {
                    throw new ValidationNotFoundException($"Validation for user with ID {customerId} not found.");
                }

                validation.Status = "deactivated";
                await _validationRepository.Update(validation);
                _logger.LogInformation($"User with ID {customerId} deactivated.");
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deactivating user: {ex.Message}");
                throw;
            }
        }


        public async Task<List<Customers>?> GetAllUsers()
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError($"Error getting all users: {ex.Message}");
                throw;
            }
        }

        public async Task<Customers?> GetUser(int customerId)
        {
            try
            {
                var customer = await _customersRepository.Get(customerId);
                if (customer == null)
                {
                    throw new NoCustomersFoundException($"User with ID {customerId} not found.");
                }
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting user with ID {customerId}: {ex.Message}");
                throw;
            }
        }

        public async Task<Customers?> UpdateCustomerContact(int customerId, AdminUpdateCustomerContactDTO contactDTO)
        {
            try
            {
                var customer = await _customersRepository.Get(customerId);
                if (customer == null)
                {
                    throw new NoCustomersFoundException($"User with ID {customerId} not found.");
                }

                AdminCustomerMapper.MapToCustomer(contactDTO, customer);
                await _customersRepository.Update(customer);
                _logger.LogInformation($"Contact details updated for user with ID {customerId}.");
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating contact details for user with ID {customerId}: {ex.Message}");
                throw;
            }
        }

        public async Task<Customers?> UpdateCustomerDetails(int customerId, AdminUpdateCustomerDetailsDTO detailsDTO)
        {
            try
            {
                var customer = await _customersRepository.Get(customerId);
                if (customer == null)
                {
                    throw new NoCustomersFoundException($"User with ID {customerId} not found.");
                }

                AdminCustomerMapper.MapToCustomer(detailsDTO, customer);
                await _customersRepository.Update(customer);
                _logger.LogInformation($"Details updated for user with ID {customerId}.");
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating details for user with ID {customerId}: {ex.Message}");
                throw;
            }
        }

        public async Task<Customers?> UpdateCustomerName(int customerId, AdminUpdateCustomerNameDTO nameDTO)
        {
            try
            {
                var customer = await _customersRepository.Get(customerId);
                if (customer == null)
                {
                    throw new NoCustomersFoundException($"User with ID {customerId} not found.");
                }

                AdminCustomerMapper.MapToCustomer(nameDTO, customer);
                await _customersRepository.Update(customer);
                _logger.LogInformation($"Name updated for user with ID {customerId}.");
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating name for user with ID {customerId}: {ex.Message}");
                throw;
            }
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

                _logger.LogInformation($"Customer created.");
                return addedCustomer;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating customer: {ex.Message}");
                throw new CustomerCreationException($"Error creating customer: {ex.Message}");
            }
        }
    }
}