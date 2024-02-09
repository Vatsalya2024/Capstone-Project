using System;
using System.Security.Cryptography;
using System.Text;
using Capstone_Project.Controllers;
using Capstone_Project.Interfaces;
using Capstone_Project.Mappers;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Repositories;

namespace Capstone_Project.Services
{
	public class AdminLoginService:IAdminLoginService
	{
        private readonly IRepository<int, Admin> _adminRepository;
        private readonly ILogger<CustomerServices> _logger;
        private readonly IRepository<string, Validation> _validationRepository;
        private readonly ITokenService _tokenService;

        public AdminLoginService(IRepository<int,Admin> adminRepository,
             ILogger<CustomerServices> logger,
            IRepository<string, Validation> validationRepository,
            ITokenService tokenService)
		{
            _adminRepository = adminRepository;
            _logger = logger;
            _validationRepository = validationRepository;
            _tokenService = tokenService;
		}

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

        public async Task<LoginUserDTO> Register(RegisterAdminDTO user)
        {
            Validation myuser = new RegisterToAdminUser(user).GetValidation();
            myuser.Status = "Active"; // Set status to "Active" by default
            myuser = await _validationRepository.Add(myuser);
            Admin admins = new RegisterToAdmin(user).GetAdmin();
            admins = await _adminRepository.Add(admins);
            LoginUserDTO result = new LoginUserDTO
            {
                Email = myuser.Email,
                UserType = myuser.UserType,
            };
            return result;
        }
    }
}

