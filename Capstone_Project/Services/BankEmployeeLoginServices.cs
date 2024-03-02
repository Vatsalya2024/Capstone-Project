using Capstone_Project.Controllers;
using Capstone_Project.Exceptions;
using Capstone_Project.Interfaces;
using Capstone_Project.Mappers;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Repositories;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Capstone_Project.Services
{
    public class BankEmployeeLoginServices : IBankEmployeeLoginService
    {
        private readonly IRepository<int, BankEmployees> _employeeRepository;
        private readonly IRepository<string, Validation> _validationRepository;
        private readonly ITokenService _tokenService;

        public BankEmployeeLoginServices(IRepository<int, BankEmployees> employeeRepository,
            IRepository<string, Validation> validationRepository,
            ITokenService tokenService)
        {
            _employeeRepository = employeeRepository;
            _validationRepository = validationRepository;
            _tokenService = tokenService;
        }

        public async Task<LoginUserDTO> Login(LoginUserDTO employee)
        {
            var myUser = await _validationRepository.Get(employee.Email);
            if (myUser == null || myUser.Status != "Active")
            {
                throw new DeactivatedUserException();
            }
            var userPassword = GetPasswordEncrypted(employee.Password, myUser.Key);
            var checkPasswordMatch = ComparePasswords(myUser.Password, userPassword);
            if (myUser.UserType == null)
            {
                throw new ValidationNotFoundException("No ValidationFound");
            }
            if (checkPasswordMatch)
            {
                employee.Password = "";
                employee.UserType = myUser.UserType;
                employee.Token = await _tokenService.GenerateToken(employee);
                return employee;
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

        public async Task<LoginUserDTO> Register(RegisterBankEmployeeDTO employee)
        {
            Validation myuser = new RegisterToBankEmployeeUser(employee).GetValidation();
            myuser.Status = "Active"; 
            myuser = await _validationRepository.Add(myuser);
            BankEmployees bankEmployees = new RegiterToBankEmployee(employee).GetBankEmployees();
            bankEmployees = await _employeeRepository.Add(bankEmployees);
            if (myuser == null || myuser.Email == null || myuser.UserType == null)
            {
                throw new ValidationNotFoundException("No Email found");
            }
            LoginUserDTO result = new LoginUserDTO
            {
                Email = myuser.Email,
                UserType = myuser.UserType,
            };
            return result;
        }
        public async Task<BankEmployees?> GetBankEmployeeInfoByEmail(string email)
        {

            var validationInfo = await _validationRepository.Get(email);

            if (validationInfo != null)
            {

                var allEmployees = await _employeeRepository.GetAll();

                if (allEmployees != null)
                {

                    var employeeInfo = allEmployees.FirstOrDefault(employee => employee.Email == email);
                    return employeeInfo;
                }
                else
                {
                    throw new NoBankEmployeesFoundException("No BankEmployees");
                }
            }
            else
            {
                throw new ValidationNotFoundException("No Email found");
            }
        }
    }
    
}