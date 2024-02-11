using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Capstone_Project.Controllers;
using Capstone_Project.Exceptions;
using Capstone_Project.Interfaces;
using Capstone_Project.Mappers;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Services
{
    public class CustomerServices : ICustomerLoginService, ICustomerAdminService
    {
        private readonly IRepository<int, Customers> _customerRepository;
        private readonly ILogger<CustomerServices> _logger;
        private readonly IRepository<string, Validation> _validationRepository;
        private readonly ITokenService _tokenService;

        public CustomerServices(IRepository<int, Customers> customerRepository,
            ILogger<CustomerServices> logger,
            IRepository<string, Validation> validationRepository,
            ITokenService tokenService)
        {
            _customerRepository = customerRepository;
            _logger = logger;
            _validationRepository = validationRepository;
            _tokenService = tokenService;
        }

        public async Task<LoginUserDTO> Login(LoginUserDTO user)
        {
            try
            {
                _logger.LogInformation("Attempting login...");

                var myUser = await _validationRepository.Get(user.Email);
                if (myUser == null || myUser.Status != "Active")
                {
                    throw new DeactivatedUserException();
                }
                var userPassword = GetPasswordEncrypted(user.Password, myUser.Key);
                var checkPasswordMatch = ComparePasswords(myUser.Password, userPassword);
                if (checkPasswordMatch)
                {
                    user.Password = "";
                    user.UserType = myUser.UserType;
                    user.Token = await _tokenService.GenerateToken(user);
                    _logger.LogInformation($"User {user.Email} logged in successfully.");
                    return user;
                }
                throw new InvalidUserException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login.");
                throw;
            }
        }

        public async Task<LoginUserDTO> Register(RegisterCustomerDTO user)
        {
            try
            {
                _logger.LogInformation("Attempting user registration...");

                Validation myuser = new RegisterToCustomerUser(user).GetValidation();
                myuser.Status = "Active"; // Set status to "Active" by default
                myuser = await _validationRepository.Add(myuser);
                Customers customers = new RegisterToCustomer(user).GetCustomers();
                customers = await _customerRepository.Add(customers);
                LoginUserDTO result = new LoginUserDTO
                {
                    Email = myuser.Email,
                    UserType = myuser.UserType,
                };
                _logger.LogInformation($"User {user.Email} registered successfully.");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during user registration.");
                throw;
            }
        }

        //public async Task<List<Customers>> GetCustomersListasync()
        //{
        //    try
        //    {
        //        _logger.LogInformation("Fetching customer list...");

        //        var customer = await _customerRepository.GetAll();
        //        return customer;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while fetching the customer list.");
        //        throw;
        //    }
        //}

        //public async Task<Customers> ChangeCustomerPhoneAsync(int id, long phone)
        //{
        //    try
        //    {
        //        _logger.LogInformation($"Changing phone number for customer with ID {id}...");

        //        var customer = await _customerRepository.Get(id);
        //        if (customer != null)
        //        {
        //            customer.PhoneNumber = phone;
        //            customer = await _customerRepository.Update(customer);
        //            return customer;
        //        }
        //        return customer;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"An error occurred while changing phone number for customer with ID {id}.");
        //        throw;
        //    }
        //}

        //public async Task<Customers> ChangeCustomerName(int id, string name)
        //{
        //    try
        //    {
        //        _logger.LogInformation($"Changing name for customer with ID {id}...");

        //        var customer = await _customerRepository.Get(id);
        //        if (customer != null)
        //        {
        //            customer.Name = name;
        //            customer = await _customerRepository.Update(customer);
        //            return customer;
        //        }
        //        return customer;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"An error occurred while changing name for customer with ID {id}.");
        //        throw;
        //    }
        //}

        //public async Task<Customers> ChangeCustomerAddress(int id, string address)
        //{
        //    try
        //    {
        //        _logger.LogInformation($"Changing address for customer with ID {id}...");

        //        var customer = await _customerRepository.Get(id);
        //        if (customer != null)
        //        {
        //            customer.Address = address;
        //            customer = await _customerRepository.Update(customer);
        //            return customer;
        //        }
        //        return customer;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"An error occurred while changing address for customer with ID {id}.");
        //        throw;
        //    }
        //}

        public async Task<Customers> ChangeCustomerPhoneAsync(int id, long phone)
        {
            try
            {
                _logger.LogInformation($"Changing phone number for customer with ID {id}...");

                var customer = await _customerRepository.Get(id);
                if (customer == null)
                {
                    throw new NoCustomersFoundException($"No customer found with ID {id}");
                }

                customer.PhoneNumber = phone;
                customer = await _customerRepository.Update(customer);
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while changing phone number for customer with ID {id}.");
                throw;
            }
        }

        public async Task<Customers> ChangeCustomerName(int id, string name)
        {
            try
            {
                _logger.LogInformation($"Changing name for customer with ID {id}...");

                var customer = await _customerRepository.Get(id);
                if (customer == null)
                {
                    throw new NoCustomersFoundException($"No customer found with ID {id}");
                }

                customer.Name = name;
                customer = await _customerRepository.Update(customer);
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while changing name for customer with ID {id}.");
                throw;
            }
        }

        public async Task<Customers> ChangeCustomerAddress(int id, string address)
        {
            try
            {
                _logger.LogInformation($"Changing address for customer with ID {id}...");

                var customer = await _customerRepository.Get(id);
                if (customer == null)
                {
                    throw new NoCustomersFoundException($"No customer found with ID {id}");
                }

                customer.Address = address;
                customer = await _customerRepository.Update(customer);
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while changing address for customer with ID {id}.");
                throw;
            }
        }

        //public async Task<Customers> DeleteCustomers(int id)
        //{
        //    try
        //    {
        //        _logger.LogInformation($"Deleting customer with ID {id}...");

        //        var customer = await _customerRepository.Get(id);
        //        if (customer == null)
        //        {
        //            throw new NoCustomersFoundException();
        //        }
        //        var result = await _customerRepository.Delete(id);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"An error occurred while deleting customer with ID {id}.");
        //        throw;
        //    }
        //}
        public async Task<Customers> DeleteCustomers(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting customer with ID {id}...");

                var customer = await _customerRepository.Get(id);
                if (customer == null)
                {
                    throw new NoCustomersFoundException($"No customer found with ID {id}");
                }

                var result = await _customerRepository.Delete(id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting customer with ID {id}.");
                throw;
            }
        }

        //public async Task<Customers> GetCustomers(int id)
        //{
        //    try
        //    {
        //        _logger.LogInformation($"Fetching customer with ID {id}...");

        //        var customer = await _customerRepository.Get(id);
        //        return customer;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"An error occurred while fetching customer with ID {id}.");
        //        throw;
        //    }
        //}
        //public async Task<List<Customers>> GetCustomersListasync()
        //{
        //    try
        //    {
        //        _logger.LogInformation("Fetching customer list...");

        //        var customer = await _customerRepository.GetAll();
        //        return customer;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while fetching the customer list.");
        //        throw;
        //    }
        //}


        //public async Task<bool> UpdateCustomerPassword(string email, string newPassword)
        //{
        //    try
        //    {
        //        _logger.LogInformation($"Updating password for customer with email {email}...");

        //        var validation = await _validationRepository.Get(email);
        //        if (validation == null)
        //        {
        //            return false;
        //        }
        //        byte[] newKey = GenerateNewKey();
        //        byte[] encryptedPassword = GetPasswordEncrypted(newPassword, newKey);
        //        validation.Password = encryptedPassword;
        //        validation.Key = newKey;
        //        await _validationRepository.Update(validation);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"An error occurred while updating password for customer with email {email}.");
        //        throw;
        //    }
        //}

        public async Task<bool> UpdateCustomerPassword(string email, string newPassword)
        {
            try
            {
                _logger.LogInformation($"Updating password for customer with email {email}...");

                var validation = await _validationRepository.Get(email);
                if (validation == null)
                {
                    throw new ValidationNotFoundException($"Validation not found for email: {email}");
                }

                byte[] newKey = GenerateNewKey();
                byte[] encryptedPassword = GetPasswordEncrypted(newPassword, newKey);
                validation.Password = encryptedPassword;
                validation.Key = newKey;
                await _validationRepository.Update(validation);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating password for customer with email {email}.");
                throw;
            }
        }


        private bool ComparePasswords(byte[] password, byte[] userPassword)
        {
            for (int i = 0; i < password.Length; i++)
            {
                if (password[i] != userPassword[i])
                    return false;
            }
            return true;
        }

        private byte[] GetPasswordEncrypted(string password, byte[] key)
        {
            HMACSHA512 hmac = new HMACSHA512(key);
            var userpassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return userpassword;
        }

        private byte[] GenerateNewKey()
        {
            byte[] newKey = new byte[64];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(newKey);
            }
            return newKey;
        }
    }
}
