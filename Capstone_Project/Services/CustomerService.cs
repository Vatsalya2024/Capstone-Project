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

        public async Task<LoginUserDTO> Login(LoginUserDTO user)
        {
            var myUser = await _validationRepository.Get(user.Email);
            if (myUser == null)
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

        public async Task<LoginUserDTO> Register(RegisterCustomerDTO user)
        {
            Validation myuser = new RegisterToCustomerUser(user).GetValidation();
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
    }
}

