using Capstone_Project.Controllers;
using Capstone_Project.Exceptions;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Capstone_Project.Services.Tests
{
    [TestFixture]
    public class BankEmployeeLoginServicesTests
    {
        private BankEmployeeLoginServices _service;
        private Mock<IRepository<int, BankEmployees>> _employeeRepositoryMock;
        private Mock<IRepository<string, Validation>> _validationRepositoryMock;
        private Mock<ITokenService> _tokenServiceMock;

        [SetUp]
        public void SetUp()
        {
            _employeeRepositoryMock = new Mock<IRepository<int, BankEmployees>>();
            _validationRepositoryMock = new Mock<IRepository<string, Validation>>();
            _tokenServiceMock = new Mock<ITokenService>();

            _service = new BankEmployeeLoginServices(
                _employeeRepositoryMock.Object,
                _validationRepositoryMock.Object,
                _tokenServiceMock.Object);
        }

        [Test]
        public async Task Login_ValidLoginUserDTO_ReturnsLoginUserDTO()
        {
            // Arrange
            var email = "test@example.com";
            var password = "password123";

            var hashedPassword = GetPasswordEncrypted(password, new byte[64]); 
            var validation = new Validation
            {
                Email = email,
                Password = hashedPassword,
                Key = new byte[64], 
                UserType = "Employee",
                Status = "Active"
            };

            var loginUserDTO = new LoginUserDTO
            {
                Email = email,
                Password = password
            };

            _validationRepositoryMock.Setup(r => r.Get(loginUserDTO.Email)).ReturnsAsync(validation);
            _tokenServiceMock.Setup(t => t.GenerateToken(It.IsAny<LoginUserDTO>())).ReturnsAsync("generated_token");

            // Act
            var result = await _service.Login(loginUserDTO);

            // Assert
            Assert.IsNotNull(result);
        }

        
        private byte[] GetPasswordEncrypted(string password, byte[] key)
        {
            using var hmac = new HMACSHA512(key);
            return hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }




        [Test]
        public async Task Register_ValidRegisterBankEmployeeDTO_ReturnsLoginUserDTO()
        {
            // Arrange
            var registerBankEmployeeDTO = new RegisterBankEmployeeDTO
            {
                Email = "test@example.com",
                UserType = "Employee",
                Password = "password",
            };
            var validation = new Validation
            {
                Email = registerBankEmployeeDTO.Email,
                UserType = registerBankEmployeeDTO.UserType,
            };
            var bankEmployees = new BankEmployees();
            _validationRepositoryMock.Setup(r => r.Add(It.IsAny<Validation>())).ReturnsAsync(validation);
            _employeeRepositoryMock.Setup(r => r.Add(It.IsAny<BankEmployees>())).ReturnsAsync(bankEmployees);

            // Act
            var result = await _service.Register(registerBankEmployeeDTO);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetBankEmployeeInfoByEmail_ValidEmail_ReturnsBankEmployee()
        {
            // Arrange
            var email = "test@example.com";
            var validationInfo = new Validation
            {
                Email = email
            };

            var bankEmployees = new List<BankEmployees>
    {
        new BankEmployees { Email = email }
        
    };

            _validationRepositoryMock.Setup(r => r.Get(email)).ReturnsAsync(validationInfo);
            _employeeRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(bankEmployees);

            // Act
            var result = await _service.GetBankEmployeeInfoByEmail(email);

            // Assert
            Assert.IsNotNull(result);
            
        }


        [Test]
        public void Login_ThrowsDeactivatedUserException()
        {
            // Arrange
            var loginUserDTO = new LoginUserDTO
            {
                Email = "deactivated@example.com",
                Password = "password"
            };

            var deactivatedValidation = new Validation
            {
                Email = "deactivated@example.com",
                Status = "Deactivated",
                Password = new byte[64],
                Key = new byte[64]
            };

            _validationRepositoryMock.Setup(repo => repo.Get(loginUserDTO.Email))
                .ReturnsAsync(deactivatedValidation);

            // Act & Assert
            Assert.ThrowsAsync<DeactivatedUserException>(() => _service.Login(loginUserDTO));
        }




       




    }
}
