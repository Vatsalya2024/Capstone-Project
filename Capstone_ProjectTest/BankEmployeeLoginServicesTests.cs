using System;
using Capstone_Project.Controllers;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Services;
using Moq;

namespace Capstone_ProjectTest
{
    [TestFixture]
    public class BankEmployeeLoginServicesTests
    {
        private Mock<IRepository<int, BankEmployees>> _employeeRepositoryMock;
        private Mock<IRepository<string, Validation>> _validationRepositoryMock;
        private Mock<ITokenService> _tokenServiceMock;
        private BankEmployeeLoginServices _bankEmployeeLoginServices;

        [SetUp]
        public void Setup()
        {
            _employeeRepositoryMock = new Mock<IRepository<int, BankEmployees>>();
            _validationRepositoryMock = new Mock<IRepository<string, Validation>>();
            _tokenServiceMock = new Mock<ITokenService>();

            _bankEmployeeLoginServices = new BankEmployeeLoginServices(
                _employeeRepositoryMock.Object,
                _validationRepositoryMock.Object,
                _tokenServiceMock.Object
            );
        }

        [Test]
        public async Task Register()
        {
            // Arrange
            var employeeDTO = new RegisterBankEmployeeDTO
            {

                Email = "v@example.com",

            };

            var validation = new Validation
            {
                Email = "v@example.com",
                Status = "Active",
                Password = new byte[64],
                Key = new byte[64]

            };

            _validationRepositoryMock.Setup(repo => repo.Add(It.IsAny<Validation>()))
                .ReturnsAsync(validation);

            var bankEmployee = new BankEmployees
            {

                EmployeeID = 1,

            };

            _employeeRepositoryMock.Setup(repo => repo.Add(It.IsAny<BankEmployees>()))
                .ReturnsAsync(bankEmployee);

            // Act
            var result = await _bankEmployeeLoginServices.Register(employeeDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Email, Is.EqualTo(employeeDTO.Email));

        }


        [Test]
        public void Login_ThrowsInvalidUserException()
        {
            // Arrange
            var loginUserDTO = new LoginUserDTO
            {
                Email = "V@gmail.com",
                Password = "wrongpassword"
            };

            var validation = new Validation
            {
                Email = "test@example.com",
                Status = "Active",
                Password = new byte[64],
                Key = new byte[64]

            };

            _validationRepositoryMock.Setup(repo => repo.Get(loginUserDTO.Email))
                .ReturnsAsync(validation);

            // Act & Assert
            Assert.ThrowsAsync<InvalidUserException>(() => _bankEmployeeLoginServices.Login(loginUserDTO));
        }


    }
}

