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
using Capstone_Project.Repositories;

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
        /// <summary>
        /// Method for Customer Login
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
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
                if (myUser.UserType == null)
                {
                    throw new ValidationNotFoundException("No ValidationFound");
                }
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
        /// <summary>
        /// Method to Resgister a Customer
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<LoginUserDTO> Register(RegisterCustomerDTO user)
        {
            try
            {
                _logger.LogInformation("Attempting user registration...");

                Validation myuser = new RegisterToCustomerUser(user).GetValidation();
                myuser.Status = "Active"; 
                myuser = await _validationRepository.Add(myuser);
                Customers customers = new RegisterToCustomer(user).GetCustomers();
                customers = await _customerRepository.Add(customers);
                if (myuser == null || myuser.Email == null || myuser.UserType == null)
                {
                    throw new ValidationNotFoundException("No Email found");
                }
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

       
        /// <summary>
        /// Method to Change the phone number of a customer
        /// </summary>
        /// <param name="id"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Method to Change the name of customer
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Method to change the Address of customer
        /// </summary>
        /// <param name="id"></param>
        /// <param name="address"></param>
        /// <returns></returns>
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

       /// <summary>
       /// Method to delete the customer
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
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
                if (result == null)
                {
                    throw new NoCustomersFoundException("No Customer found");
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting customer with ID {id}.");
                throw;
            }
        }

       
        /// <summary>
        /// Meethod to update the customer password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Method to reset password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="newPassword"></param>
        /// <param name="confirmPassword"></param>
        /// <returns></returns>
        /// <exception cref="PasswordMismatchException"></exception>
        /// <exception cref="ValidationNotFoundException"></exception>
        public async Task<bool> ResetPassword(string email, string newPassword, string confirmPassword)
        {
            _logger.LogInformation($"Resetting password for customer with email {email}...");

            
            if (newPassword != confirmPassword)
            {
                throw new PasswordMismatchException("New password and confirm password do not match.");
            }

            
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

            _logger.LogInformation($"Password for customer with email {email} reset successfully.");
            return true;
        }


        public async Task<Customers?> GetCustomerInfoByEmail(string email)
        {
            
            var validationInfo = await _validationRepository.Get(email);

            if (validationInfo != null)
            {
                
                var allCustomers = await _customerRepository.GetAll();

                if (allCustomers != null)
                {
                    
                    var customerInfo = allCustomers.FirstOrDefault(customer => customer.Email == email);
                    return customerInfo;
                }
                else
                {
                    throw new NoCustomersFoundException("No Customers");
                }
            }
            else
            {
                throw new ValidationNotFoundException("No Email found");
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
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(newKey);
            }
            return newKey;
        }

    }
}
