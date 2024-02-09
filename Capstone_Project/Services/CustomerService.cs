using System;
using Capstone_Project.Interfaces;
using Capstone_Project.Mappers;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using System.Security.Cryptography;
using System.Text;
using Capstone_Project.Controllers;

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

        //public async Task<LoginUserDTO> Login(LoginUserDTO user)
        //{
        //    var myUser = await _validationRepository.Get(user.Email);
        //    if (myUser == null)
        //    {
        //        throw new InvalidUserException();
        //    }
        //    var userPassword = GetPasswordEncrypted(user.Password, myUser.Key);
        //    var checkPasswordMatch = ComparePasswords(myUser.Password, userPassword);
        //    if (checkPasswordMatch)
        //    {
        //        user.Password = "";
        //        user.UserType = myUser.UserType;
        //        user.Token = await _tokenService.GenerateToken(user);
        //        return user;
        //    }
        //    throw new InvalidUserException();
        //}


        public async Task<LoginUserDTO> Login(LoginUserDTO user)
        {
            var myUser = await _validationRepository.Get(user.Email);
            if (myUser == null || myUser.Status != "Active")
            {
                throw new InvalidUserException();
            }
            var userPassword = GetPasswordEncrypted(user.Password, myUser.Key);
            var checkPasswordMatch = ComparePasswords(myUser.Password, userPassword);
            if (checkPasswordMatch)
            {
                user.Password = "";
                user.UserType = myUser.UserType;
                user.Token = await _tokenService.GenerateToken(user);
                return user;
            }
            throw new InvalidUserException();
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

        //public async Task<LoginUserDTO> Register(RegisterCustomerDTO user)
        //{
        //    Validation myuser = new RegisterToCustomerUser(user).GetValidation();
        //    myuser = await _validationRepository.Add(myuser);
        //    Customers customers = new RegisterToCustomer(user).GetCustomers();
        //    customers = await _customerRepository.Add(customers);
        //    LoginUserDTO result = new LoginUserDTO
        //    {
        //        Email = myuser.Email,
        //        UserType = myuser.UserType,
        //    };
        //    return result;
        //}


        public async Task<LoginUserDTO> Register(RegisterCustomerDTO user)
        {
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
            return result;
        }


        public async Task<List<Customers>> GetCustomersListasync()
        {
            var customer = await _customerRepository.GetAll();
            return customer;
        }

        public async Task<Customers> ChangeCustomerPhoneAsync(int id, long phone)
        {
            var customer = await _customerRepository.Get(id);
            if (customer != null)
            {
                customer.PhoneNumber = phone;
                customer = await _customerRepository.Update(customer);
                return customer;
            }
            return customer;
        }

        public async Task<Customers> ChangeCustomerName(int id, string name)
        {
            var customer = await _customerRepository.Get(id);
            if (customer != null)
            {
                customer.Name = name;
                customer = await _customerRepository.Update(customer);
                return customer;
            }
            return customer;
        }

        public async Task<Customers> ChangeCustomerAddress(int id, string address)
        {
            var customer = await _customerRepository.Get(id);
            if (customer != null)
            {
                customer.Address = address;
                customer = await _customerRepository.Update(customer);
                return customer;
            }
            return customer;
        }

        public async Task<Customers> DeleteCustomers(int id)
        {
            var customer = await _customerRepository.Get(id);
            if (customer == null)
            {
                throw new NoCustomersFoundException();
            }
            var result = await _customerRepository.Delete(id);
            return result;
        }

        public async Task<Customers> GetCustomers(int id)
        {
            var customer = await _customerRepository.Get(id);
            return customer;
        }

        public async Task<bool> UpdateCustomerPassword(string email, string newPassword)
        {
            // Fetch the validation information for the customer by email
            var validation = await _validationRepository.Get(email);

            // Check if the validation information exists
            if (validation == null)
            {
                // Customer not found
                return false;
            }

            // Generate a new key for password encryption (assuming it's required)
            byte[] newKey = GenerateNewKey(); // Implement the method to generate a new key

            // Encrypt the new password using the new key
            byte[] encryptedPassword = GetPasswordEncrypted(newPassword, newKey);

            // Update the validation information with the new password and key
            validation.Password = encryptedPassword;
            validation.Key = newKey;

            // Save the updated validation information back to the repository
            await _validationRepository.Update(validation);

            return true; // Password updated successfully
        }

        // Method to generate a new key for password encryption
        private byte[] GenerateNewKey()
        {
            // Generate a new random key using cryptographic functions
            byte[] newKey = new byte[64]; // Assuming the key size is 64 bytes
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(newKey);
            }
            return newKey;
        }

    }
}

