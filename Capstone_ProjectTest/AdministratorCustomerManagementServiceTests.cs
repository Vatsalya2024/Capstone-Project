using System;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Capstone_ProjectTest
{
    [TestFixture]
    public class AdministratorCustomerManagementServiceTests
    {
        private Mock<IRepository<int, Customers>> _customersRepositoryMock;
        private Mock<IRepository<string, Validation>> _validationRepositoryMock;
        private Mock<ILogger<AdministratorCustomerManagementService>> _loggerMock;
        private AdministratorCustomerManagementService _adminCustomerService;

        [SetUp]
        public void Setup()
        {
            _customersRepositoryMock = new Mock<IRepository<int, Customers>>();
            _validationRepositoryMock = new Mock<IRepository<string, Validation>>();
            _loggerMock = new Mock<ILogger<AdministratorCustomerManagementService>>();
            _adminCustomerService = new AdministratorCustomerManagementService(
                _customersRepositoryMock.Object,
                _validationRepositoryMock.Object,
                _loggerMock.Object);
        }

        [Test]
        public async Task ActivateUser()
        {
            // Arrange
            int customerId = 1;
            var customer = new Customers { CustomerID = customerId, Name = "V",Email= "v@example.com" };
            var validation = new Validation { Email = "v@example.com", Status = "Inactive" };

            _customersRepositoryMock.Setup(repo => repo.Get(customerId))
                .ReturnsAsync(customer);
            _validationRepositoryMock.Setup(repo => repo.Get(customer.Email))
                .ReturnsAsync(validation);
            _validationRepositoryMock.Setup(repo => repo.Update(validation))
                .ReturnsAsync(validation);

            // Act
            var result = await _adminCustomerService.ActivateUser(customerId);

            // Assert
            Assert.IsNotNull(result);
           
        }

        [Test]
        public async Task DeactivateUser()
        {
            // Arrange
            int customerId = 1;
            var customer = new Customers { CustomerID = customerId, Name = "v",Email= "v@example.com" };
            var validation = new Validation { Email = "v@example.com", Status = "Active" };

            _customersRepositoryMock.Setup(repo => repo.Get(customerId))
                .ReturnsAsync(customer);
            _ = _validationRepositoryMock.Setup(repo => repo.Get(customer.Email))
                .ReturnsAsync(validation);
            _validationRepositoryMock.Setup(repo => repo.Update(validation))
                .ReturnsAsync(validation);

            // Act
            var result = await _adminCustomerService.DeactivateUser(customerId);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.CustomerID, Is.EqualTo(customerId));
           
        }

        [Test]
        public async Task GetAllUsers()
        {
            // Arrange
            var validations = new List<Validation>
            {
                new Validation { Email = "v@gmail.com", UserType = "Customer" },
                new Validation { Email = "b@example.com", UserType = "Customer" }
            };
            var customers = new List<Customers>
            {
                new Customers { CustomerID = 1, Name = "Customer 1", Email = "v@gmail.com" },
                new Customers { CustomerID = 2, Name = "Customer 2", Email = "b@example.com" }
            };

            _validationRepositoryMock.Setup(repo => repo.GetAll())
                .ReturnsAsync(validations);
            _customersRepositoryMock.Setup(repo => repo.GetAll())
                .ReturnsAsync(customers);

            // Act
            var result = await _adminCustomerService.GetAllUsers();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));

        }

        [Test]
        public async Task GetUser()
        {
            // Arrange
            var customerId = 1;
            var expectedCustomer = new Customers { CustomerID = customerId, Name = "Test Customer", Email = "v@e.com" };
            _customersRepositoryMock.Setup(repo => repo.Get(customerId))
                .ReturnsAsync(expectedCustomer);

            // Act
            var result = await _adminCustomerService.GetUser(customerId);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(expectedCustomer));
        }

        [Test]
        public void ActivateUser_CustomerNotFound_ThrowsNoCustomersFoundException()
        {
            // Arrange
            int customerId = 1;
            _customersRepositoryMock.Setup(repo => repo.Get(customerId))
                .ReturnsAsync((Customers?)null);

            // Act & Assert
            Assert.ThrowsAsync<NoCustomersFoundException>(async () => await _adminCustomerService.ActivateUser(customerId));
        }



        [Test]
        public void DeactivateUser_CustomerNotFound_ThrowsNoCustomersFoundException()
        {
            // Arrange
            int customerId = 1;
            _customersRepositoryMock.Setup(repo => repo.Get(customerId))
                .ReturnsAsync((Customers?)null);

            // Act & Assert
            Assert.ThrowsAsync<NoCustomersFoundException>(async () => await _adminCustomerService.DeactivateUser(customerId));
        }





        [Test]
        public void GetUser_CustomerNotFound_ThrowsNoCustomersFoundException()
        {
            // Arrange
            int customerId = 1;
            _customersRepositoryMock.Setup(repo => repo.Get(customerId))
                .ReturnsAsync((Customers?)null);

            // Act & Assert
            Assert.ThrowsAsync<NoCustomersFoundException>(async () => await _adminCustomerService.GetUser(customerId));
        }

        [Test]
        public void UpdateCustomerContact_CustomerNotFound_ThrowsNoCustomersFoundException()
        {
            // Arrange
            int customerId = 1;
            var contactDTO = new AdminUpdateCustomerContactDTO { PhoneNumber = 1234567890 };

            _customersRepositoryMock.Setup(repo => repo.Get(customerId))
                .ReturnsAsync((Customers?)null);

            // Act & Assert
            Assert.ThrowsAsync<NoCustomersFoundException>(async () => await _adminCustomerService.UpdateCustomerContact(customerId, contactDTO));
        }

        [Test]
        public void UpdateCustomerDetails_CustomerNotFound_ThrowsNoCustomersFoundException()
        {
            // Arrange
            int customerId = 1;
            var detailsDTO = new AdminUpdateCustomerDetailsDTO
            {
                DOB = new DateTime(2002, 1, 1),
                Age = 30,
                PANNumber = "ABCDE1234F",
                Gender = "Male"
            };

            _customersRepositoryMock.Setup(repo => repo.Get(customerId))
                .ReturnsAsync((Customers?)null);

            // Act & Assert
            Assert.ThrowsAsync<NoCustomersFoundException>(async () => await _adminCustomerService.UpdateCustomerDetails(customerId, detailsDTO));
        }

        [Test]
        public void UpdateCustomerName_CustomerNotFound_ThrowsNoCustomersFoundException()
        {
            // Arrange
            int customerId = 1;
            var nameDTO = new AdminUpdateCustomerNameDTO { Name = "T" };

            _customersRepositoryMock.Setup(repo => repo.Get(customerId))
                .ReturnsAsync((Customers?)null);

            // Act & Assert
            Assert.ThrowsAsync<NoCustomersFoundException>(async () => await _adminCustomerService.UpdateCustomerName(customerId, nameDTO));
        }




        [Test]
        public async Task UpdateCustomerContact()
        {
            // Arrange
            var customerId = 1;
            var contactDTO = new AdminUpdateCustomerContactDTO { PhoneNumber = 1234567890 };
            var originalCustomer = new Customers { CustomerID = customerId, Name = " Customer", Email = "v@g.com" };
            var updatedCustomer = new Customers { CustomerID = customerId, Name = " Customer", Email = "v@g.com", PhoneNumber = 1234567890 };

            _customersRepositoryMock.Setup(repo => repo.Get(customerId))
                .ReturnsAsync(originalCustomer);

            // Act
            var result = await _adminCustomerService.UpdateCustomerContact(customerId, contactDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(updatedCustomer));
        }

        [Test]
        public async Task UpdateCustomerDetails()
        {
            // Arrange
            var customerId = 1;
            var mockCustomersRepository = new Mock<IRepository<int, Customers>>();
            var mockValidationRepository = new Mock<IRepository<string, Validation>>();
            var mockLogger = new Mock<ILogger<AdministratorCustomerManagementService>>();

            var service = new AdministratorCustomerManagementService(
                mockCustomersRepository.Object,
                mockValidationRepository.Object,
                mockLogger.Object
            );

            var detailsDTO = new AdminUpdateCustomerDetailsDTO
            {
                DOB = new DateTime(2002, 1, 1),
                Age = 30,
                PANNumber = "ABCDE1234F",
                Gender = "Male"
            };

            var existingCustomer = new Customers
            {
                CustomerID = customerId,
                Name = "v",
                Email = "v@e.com"


            };

            mockCustomersRepository.Setup(repo => repo.Get(customerId))
                                   .ReturnsAsync(existingCustomer);

            // Act
            var updatedCustomer = await service.UpdateCustomerDetails(customerId, detailsDTO);

            // Assert
            Assert.IsNotNull(updatedCustomer);
            Assert.That(updatedCustomer.DOB, Is.EqualTo(detailsDTO.DOB));
            }
        [Test]
        public async Task UpdateCustomerName()
        {
            // Arrange
            var customerId = 1;
            var mockCustomersRepository = new Mock<IRepository<int, Customers>>();
            var mockValidationRepository = new Mock<IRepository<string, Validation>>();
            var mockLogger = new Mock<ILogger<AdministratorCustomerManagementService>>();

            var service = new AdministratorCustomerManagementService(
                mockCustomersRepository.Object,
                mockValidationRepository.Object,
                mockLogger.Object
            );

            var nameDTO = new AdminUpdateCustomerNameDTO
            {
                Name = "T"
            };

            var existingCustomer = new Customers
            {
                CustomerID = customerId,
                Name = "V",
                Email = "v@e.com"

            };

            mockCustomersRepository.Setup(repo => repo.Get(customerId))
                                   .ReturnsAsync(existingCustomer);

            // Act
            var updatedCustomer = await service.UpdateCustomerName(customerId, nameDTO);

            // Assert
            Assert.IsNotNull(updatedCustomer);
            Assert.That(updatedCustomer.Name, Is.EqualTo(nameDTO.Name));

            Assert.That(updatedCustomer.Email, Is.EqualTo(existingCustomer.Email));
        }

        [Test]
        public async Task CreateCustomer()
        {
            // Arrange
            var mockCustomersRepository = new Mock<IRepository<int, Customers>>();
            var mockValidationRepository = new Mock<IRepository<string, Validation>>();
            var mockLogger = new Mock<ILogger<AdministratorCustomerManagementService>>();

            var service = new AdministratorCustomerManagementService(
                mockCustomersRepository.Object,
                mockValidationRepository.Object,
                mockLogger.Object
            );

            var customerDTO = new RegisterCustomerDTO
            {

                Name = "V",
                Email = "V@b.com",

            };

            // Mock repository behavior
            mockValidationRepository.Setup(repo => repo.Add(It.IsAny<Validation>()))
           .ReturnsAsync(new Validation { Email = customerDTO.Email });

            mockCustomersRepository.Setup(repo => repo.Add(It.IsAny<Customers>()))
            .ReturnsAsync(new Customers { CustomerID = 1, Name = customerDTO.Name, Email = customerDTO.Email });

            // Act
            var createdCustomer = await service.CreateCustomer(customerDTO);

            // Assert
            Assert.IsNotNull(createdCustomer);
            Assert.That(createdCustomer.Name, Is.EqualTo(customerDTO.Name));
            

        }


    }
}

