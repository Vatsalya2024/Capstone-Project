using System;
using Capstone_Project.Controllers;
using Capstone_Project.Exceptions;
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
        }


        [Test]
        public async Task ChangeCustomerPhoneAsync_CustomerNotFound_ThrowsNoCustomersFoundException()
        {
            // Arrange
            int customerId = 1;
            long newPhoneNumber = 9876543210;


            _mockCustomerRepository.Setup(repo => repo.Get(customerId)).ReturnsAsync((Customers?)null);

            // Act and Assert
            // var exception = await Assert.ThrowsAsync<NoCustomersFoundException>(() =>/* _customerServices.ChangeCustomerPhoneAsync(customerId, newPhoneNumber))*/;
            try { await _customerServices.ChangeCustomerPhoneAsync(customerId, newPhoneNumber); }
            catch (NoCustomersFoundException)
            {
                Assert.IsTrue(true);
                return;
            }
            Assert.Fail("Expected Exception is not thrown NoCustomersFoundException");


        }
        [Test]
        public void ChangeCustomerName_CustomerNotFound_ThrowsNoCustomersFoundException()
        {
            // Arrange
            int customerId = 1;
            string newName = "John";


            _mockCustomerRepository.Setup(repo => repo.Get(customerId)).ReturnsAsync((Customers?)null);

            // Act and Assert
            Assert.ThrowsAsync<NoCustomersFoundException>(() => _customerServices.ChangeCustomerName(customerId, newName));
        }

        [Test]
        public void ChangeCustomerAddress_CustomerNotFound_ThrowsNoCustomersFoundException()
        {
            // Arrange
            int customerId = 1;
            string newAddress = "123 Main St";


            _mockCustomerRepository.Setup(repo => repo.Get(customerId)).ReturnsAsync((Customers?)null);

            // Act and Assert
            Assert.ThrowsAsync<NoCustomersFoundException>(() => _customerServices.ChangeCustomerAddress(customerId, newAddress));
        }

        [Test]
        public void DeleteCustomers_CustomerNotFound_ThrowsNoCustomersFoundException()
        {
            // Arrange
            int customerId = 1;


            _mockCustomerRepository.Setup(repo => repo.Get(customerId)).ReturnsAsync((Customers?)null);

            // Act and Assert
            Assert.ThrowsAsync<NoCustomersFoundException>(() => _customerServices.DeleteCustomers(customerId));
        }

        [Test]
        public void UpdateCustomerPassword_ValidationNotFound_ThrowsValidationNotFoundException()
        {
            // Arrange
            string email = "test@example.com";
            string newPassword = "newPassword";


            _mockValidationRepository.Setup(repo => repo.Get(email)).ReturnsAsync((Validation?)null);

            // Act and Assert
            Assert.ThrowsAsync<ValidationNotFoundException>(() => _customerServices.UpdateCustomerPassword(email, newPassword));
        }

        [Test]
        public void ResetPassword_PasswordMismatch_ThrowsPasswordMismatchException()
        {
            // Arrange
            string email = "test@example.com";
            string newPassword = "newPassword";
            string confirmPassword = "mismatchedPassword";

            // Act and Assert
            Assert.ThrowsAsync<PasswordMismatchException>(() => _customerServices.ResetPassword(email, newPassword, confirmPassword));
        }

        [Test]
        public void ResetPassword_ValidationNotFound_ThrowsValidationNotFoundException()
        {
            // Arrange
            string email = "test@example.com";
            string newPassword = "newPassword";
            string confirmPassword = "newPassword";


            _mockValidationRepository.Setup(repo => repo.Get(email)).ReturnsAsync((Validation?)null);

            // Act and Assert
            Assert.ThrowsAsync<ValidationNotFoundException>(() => _customerServices.ResetPassword(email, newPassword, confirmPassword));
        }
        [Test]
        public async Task GetCustomerInfoByEmail_ValidEmail_ReturnsCustomerInfo()
        {
            // Arrange
            string email = "test@example.com";
            var validationInfo = new Validation
            {
                Email = email,
                Password = new byte[64],
                Key = new byte[64],
                UserType = "Customer",
                Status = "Active"
            };

            var allCustomers = new List<Customers>
    {
        new Customers { CustomerID = 1, Name = "John", Email = email, PhoneNumber = 1234567890, Address = "123 Main St" },
        new Customers { CustomerID = 2, Name = "Jane", Email = "jane@example.com", PhoneNumber = 9876543210, Address = "456 Elm St" }
    };

            _mockValidationRepository.Setup(repo => repo.Get(email)).ReturnsAsync(validationInfo);
            _mockCustomerRepository.Setup(repo => repo.GetAll()).ReturnsAsync(allCustomers);

            // Act
            var result = await _customerServices.GetCustomerInfoByEmail(email);

            // Assert

            Assert.That(result?.Name, Is.EqualTo("John"));
        }
        [Test]
        public void GetCustomerInfoByEmail_ValidationNotFoundExceptionThrown_ThrowsValidationNotFoundException()
        {
            // Arrange
            string email = "nonexistent@example.com";
            _mockValidationRepository.Setup(repo => repo.Get(email)).ThrowsAsync(new ValidationNotFoundException("Email not found"));

            // Act and Assert
            var exception = Assert.Throws<ValidationNotFoundException>(() => _customerServices.GetCustomerInfoByEmail(email).GetAwaiter().GetResult());

            Assert.That(exception.Message, Is.EqualTo("Email not found"));
        }


        [Test]
        public void Login_DeactivatedUser_ThrowsDeactivatedUserException()
        {
            // Arrange
            var loginUserDTO = new LoginUserDTO
            {
                Email = "deactivated@example.com",
                Password = "password"
            };

            var deactivatedUser = new Validation
            {
                Email = loginUserDTO.Email,
                Status = "Deactivated"
            };

            _mockValidationRepository.Setup(repo => repo.Get(loginUserDTO.Email)).ReturnsAsync(deactivatedUser);

            // Act and Assert
            Assert.ThrowsAsync<DeactivatedUserException>(() => _customerServices.Login(loginUserDTO));
        }




        [Test]
        public void RegisterUserDTO_ValidationRepositoryThrowsException_ThrowsException()
        {
            // Arrange
            var registerDTO = new RegisterCustomerDTO
            {
                Email = "error@example.com",
                Password = "password",
                UserType = "Customer"
            };

            _mockValidationRepository.Setup(repo => repo.Add(It.IsAny<Validation>())).ThrowsAsync(new Exception("Validation repository error"));

            // Act and Assert
            Assert.ThrowsAsync<Exception>(() => _customerServices.Register(registerDTO));
        }

        [Test]
        public void ChangeCustomerPhoneAsync_InvalidId_ThrowsNoCustomersFoundException()
        {
            // Arrange
            int invalidCustomerId = -1;
            long newPhoneNumber = 9876543210;

            // Act and Assert
            Assert.ThrowsAsync<NoCustomersFoundException>(() => _customerServices.ChangeCustomerPhoneAsync(invalidCustomerId, newPhoneNumber));
        }






    }
}

