using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Capstone_Project.Controllers;
using Capstone_Project.Interfaces;
using Capstone_Project.Mappers;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Repositories;

namespace Capstone_Project.Services
{
    public class AdminLoginService : IAdminLoginService
    {
        private readonly IRepository<int, Admin> _adminRepository;
        private readonly ILogger<CustomerServices> _logger;
        private readonly IRepository<string, Validation> _validationRepository;
        private readonly ITokenService _tokenService;

        public AdminLoginService(IRepository<int, Admin> adminRepository,
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
            try
            {
                _logger.LogInformation("Attempting to log in user with email: {0}", user.Email);

                var myUser = await _validationRepository.Get(user.Email);
                if (myUser == null || myUser.Status != "Active")
                {
                    _logger.LogWarning("Invalid user login attempt for email: {0}", user.Email);
                    throw new InvalidUserException();
                }

                var userPassword = GetPasswordEncrypted(user.Password, myUser.Key);
                var checkPasswordMatch = ComparePasswords(myUser.Password, userPassword);
                if (myUser.UserType == null)
                {
                    throw new ValidationNotFoundException("No ValidationFound");
                }
                if (checkPasswordMatch)
                {
                    _logger.LogInformation("User logged in successfully: {0}", user.Email);
                    user.Password = "";
                    user.UserType = myUser.UserType;
                    user.Token = await _tokenService.GenerateToken(user);
                    return user;
                }

                _logger.LogWarning("Invalid password for user: {0}", user.Email);
                throw new InvalidUserException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while logging in user: {0}", user.Email);
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

        public async Task<LoginUserDTO> Register(RegisterAdminDTO user)
        {
            try
            {
                _logger.LogInformation("Registering new admin with email: {0}", user.Email);

                Validation myuser = new RegisterToAdminUser(user).GetValidation();
                myuser.Status = "Active"; 
                myuser = await _validationRepository.Add(myuser);
                Admin admins = new RegisterToAdmin(user).GetAdmin();
                admins = await _adminRepository.Add(admins);
                if (myuser == null || myuser.Email == null || myuser.UserType == null)
                {
                    throw new ValidationNotFoundException("No Email found");
                }
                LoginUserDTO result = new LoginUserDTO
                {
                    Email = myuser.Email,
                    UserType = myuser.UserType,
                };

                _logger.LogInformation("Admin registered successfully: {0}", user.Email);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering admin: {0}", user.Email);
                throw;
            }
        }
    }
}
