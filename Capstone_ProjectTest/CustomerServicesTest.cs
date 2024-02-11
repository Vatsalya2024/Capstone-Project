using System;
using Capstone_Project.Controllers;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Capstone_ProjectTest
{
    public class CustomerServicesTest
    {
        private Mock<IRepository<int, Customers>> _mockCustomerRepository;
        private Mock<ILogger<CustomerServices>> _mockLogger;
        private Mock<IRepository<string, Validation>> _mockValidationRepository;
        private Mock<ITokenService> _mockTokenService;
        private CustomerServices _customerServices;

        [SetUp]
        public void Setup()
        {
            _mockCustomerRepository = new Mock<IRepository<int, Customers>>();
            _mockLogger = new Mock<ILogger<CustomerServices>>();
            _mockValidationRepository = new Mock<IRepository<string, Validation>>();
            _mockTokenService = new Mock<ITokenService>();
            _customerServices = new CustomerServices(
                _mockCustomerRepository.Object,
                _mockLogger.Object,
                _mockValidationRepository.Object,
                _mockTokenService.Object);
        }

        [Test]
        public async Task RegisterUserDTO()
        {
            // Arrange
            var registerDTO = new RegisterCustomerDTO
            {
                Email = "test@example.com",
                Password = "password",
                UserType = "Customer",
                CustomerID = 1,
                Name = "John Doe",
                DOB = new DateTime(1990, 1, 1),
                Age = 32,
                PhoneNumber = 1234567890,
                Address = "123 Main St",
                AadharNumber = 123456789012L,
                PANNumber = "ABCDE1234F",
                Gender = "Male"
            };

            var validation = new Validation
            {
                Email = registerDTO.Email,
                Password = new byte[64],
                UserType = registerDTO.UserType,
                Status = "Active"
            };

            var customer = new Customers
            {
                CustomerID = registerDTO.CustomerID,
                Name = registerDTO.Name,
                DOB = registerDTO.DOB,
                Age = registerDTO.Age,
                PhoneNumber = registerDTO.PhoneNumber,
                Address = registerDTO.Address,
                AadharNumber = registerDTO.AadharNumber,
                PANNumber = registerDTO.PANNumber,
                Gender = registerDTO.Gender
            };

            _mockValidationRepository.Setup(repo => repo.Add(It.IsAny<Validation>())).ReturnsAsync(validation);
            _mockCustomerRepository.Setup(repo => repo.Add(It.IsAny<Customers>())).ReturnsAsync(customer);
            _mockTokenService.Setup(service => service.GenerateToken(It.IsAny<LoginUserDTO>())).ReturnsAsync("sample_token");

            // Act
            var result = await _customerServices.Register(registerDTO);

            // Assert
            Assert.IsNotNull(result);

        }
       
        [Test]
        public async Task ChangeCustomerPhoneAsync_ValidId_ReturnsUpdatedCustomer()
        {
            // Arrange
            int customerId = 1;
            long newPhoneNumber = 9876543210;
            var customerToUpdate = new Customers { CustomerID = customerId, Name = "V", PhoneNumber = 1234567890 };
            var updatedCustomer = new Customers { CustomerID = customerId, Name = "V", PhoneNumber = newPhoneNumber };

            _mockCustomerRepository.Setup(repo => repo.Get(customerId)).ReturnsAsync(customerToUpdate);
            _mockCustomerRepository.Setup(repo => repo.Update(It.IsAny<Customers>())).ReturnsAsync(updatedCustomer);

            // Act
            var result = await _customerServices.ChangeCustomerPhoneAsync(customerId, newPhoneNumber);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(updatedCustomer.PhoneNumber, result.PhoneNumber);
            // Add more assertions based on expected behavior
        }
        [Test]
        public async Task ChangeCustomerName_ValidId_ReturnsUpdatedCustomer()
        {
            // Arrange
            int customerId = 1;
            string newName = "H";
            var customerToUpdate = new Customers { CustomerID = customerId, Name = "V", PhoneNumber = 1234567890 };
            var updatedCustomer = new Customers { CustomerID = customerId, Name = newName, PhoneNumber = 1234567890 };

            _mockCustomerRepository.Setup(repo => repo.Get(customerId)).ReturnsAsync(customerToUpdate);
            _mockCustomerRepository.Setup(repo => repo.Update(It.IsAny<Customers>())).ReturnsAsync(updatedCustomer);

            // Act
            var result = await _customerServices.ChangeCustomerName(customerId, newName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(updatedCustomer.Name, result.Name);
            // Add more assertions based on expected behavior
        }
        [Test]
        public async Task ChangeCustomerAddress_ValidId_ReturnsUpdatedCustomer()
        {
            // Arrange
            int customerId = 1;
            string newAddress = "Dehradun";
            var customerToUpdate = new Customers { CustomerID = customerId, Name = "V", Address = "456 Elm St", PhoneNumber = 1234567890 };
            var updatedCustomer = new Customers { CustomerID = customerId, Name = "V", Address = newAddress, PhoneNumber = 1234567890 };

            _mockCustomerRepository.Setup(repo => repo.Get(customerId)).ReturnsAsync(customerToUpdate);
            _mockCustomerRepository.Setup(repo => repo.Update(It.IsAny<Customers>())).ReturnsAsync(updatedCustomer);

            // Act
            var result = await _customerServices.ChangeCustomerAddress(customerId, newAddress);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(updatedCustomer.Address, result.Address);
            // Add more assertions based on expected behavior
        }
        [Test]
        public async Task DeleteCustomers_CustomerExists_ReturnsDeletedCustomer()
        {
            // Arrange
            int customerId = 1;
            var customerToDelete = new Customers { CustomerID = customerId, Name = "V", Address = "123 ", PhoneNumber = 1234567890 };

            _mockCustomerRepository.Setup(repo => repo.Get(customerId)).ReturnsAsync(customerToDelete);
            _mockCustomerRepository.Setup(repo => repo.Delete(customerId)).ReturnsAsync(customerToDelete);

            // Act
            var result = await _customerServices.DeleteCustomers(customerId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(customerToDelete, result);
            // Add more assertions based on expected behavior
        }

        [Test]
        public void DeleteCustomers_CustomerDoesNotExist_ThrowsNoCustomersFoundException()
        {
            // Arrange
            int customerId = 1;

            _mockCustomerRepository.Setup(repo => repo.Get(customerId)).ReturnsAsync((Customers)null);

            // Act & Assert
            Assert.ThrowsAsync<NoCustomersFoundException>(async () => await _customerServices.DeleteCustomers(customerId));
        }

        [Test]
        public async Task UpdateCustomerPassword_ValidEmail_ReturnsTrue()
        {
            // Arrange
            string email = "test@example.com";
            string newPassword = "newPassword";

            var validation = new Validation
            {
                Email = email,
                Password = new byte[64], // Assuming password is already encrypted
                Key = new byte[64] // Assuming key is already generated
            };

            _mockValidationRepository.Setup(repo => repo.Get(email)).ReturnsAsync(validation);
            _mockValidationRepository.Setup(repo => repo.Update(validation)).ReturnsAsync(validation);

            // Act
            var result = await _customerServices.UpdateCustomerPassword(email, newPassword);

            // Assert
            Assert.IsTrue(result);
            
        }

     

    }
}

