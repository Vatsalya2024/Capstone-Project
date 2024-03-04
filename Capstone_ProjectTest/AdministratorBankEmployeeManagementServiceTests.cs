using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone_Project.Context;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Repositories;
using Capstone_Project.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Capstone_Project.Tests
{
    [TestFixture]
    public class AdministratorBankEmployeeManagementServiceTests
    {
        private DbContextOptions<MavericksBankContext> _options;
        private MavericksBankContext _context;
        private IRepository<int, BankEmployees> _mockBankEmployeesRepository;
        private IRepository<string, Validation> _mockValidationRepository;
        private ILogger<AdministratorBankEmployeeManagementService> _mockLogger;
        private AdministratorBankEmployeeManagementService _service;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<MavericksBankContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new MavericksBankContext(_options);

            _mockBankEmployeesRepository = new BankEmployeesRepository(_context, Mock.Of<ILogger<BankEmployeesRepository>>());
            _mockValidationRepository = new ValidationRepository(_context, Mock.Of<ILogger<ValidationRepository>>());
            _mockLogger = Mock.Of<ILogger<AdministratorBankEmployeeManagementService>>();

            _service = new AdministratorBankEmployeeManagementService(
                _mockBankEmployeesRepository,
                _mockValidationRepository,
                _mockLogger
            );
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task ActivateEmployee_ValidEmployeeId_ReturnsActivatedEmployee()
        {
            // Arrange
            var employeeId = 1;
            var email = "test@example.com";

            var employee = new BankEmployees { EmployeeID = employeeId, Email = email, Name = "John Doe", Position = "Manager", Phone = "1234567890" };
            var validation = new Validation { Email = email, Status = "Inactive", Password = new byte[] { 1, 2, 3 }, UserType = "BankEmployee", Key = new byte[] { 4, 5, 6 } };

            await _context.BankEmployees.AddAsync(employee);
            await _context.Validation.AddAsync(validation);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.ActivateEmployee(employeeId);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task DeactivateEmployee_ValidEmployeeId_ReturnsDeactivatedEmployee()
        {
            // Arrange
            var employeeId = 1;
            var email = "test@example.com";

            var employee = new BankEmployees { EmployeeID = employeeId, Email = email, Name = "John Doe", Position = "Manager", Phone = "1234567890" };
            var validation = new Validation { Email = email, Status = "Active", Password = new byte[] { 1, 2, 3 }, UserType = "BankEmployee", Key = new byte[] { 4, 5, 6 } };

            await _context.BankEmployees.AddAsync(employee);
            await _context.Validation.AddAsync(validation);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.DeactivateEmployee(employeeId);

            // Assert
            Assert.IsNotNull(result);
        }

       

        [Test]
        public async Task GetEmployee_ValidEmployeeId_ReturnsEmployee()
        {
            // Arrange
            var employeeId = 1;
            var email = "test@example.com";

            var employee = new BankEmployees { EmployeeID = employeeId, Email = email, Name = "John Doe", Position = "Manager", Phone = "1234567890" };

            await _context.BankEmployees.AddAsync(employee);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetEmployee(employeeId);

            // Assert
            Assert.IsNotNull(result);
        }
        [Test]
        public async Task CreateBankEmployee_ValidData_ReturnsCreatedEmployee()
        {
            // Arrange
            var employeeDTO = new RegisterBankEmployeeDTO
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Position = "Manager",
                Phone = "1234567890"
            };

            // Act
            var result = await _service.CreateBankEmployee(employeeDTO);

            // Assert
            Assert.IsNotNull(result);
            // Add more assertions as per your implementation
        }

        [Test]
        public async Task UpdateEmployee_ValidEmployee_ReturnsUpdatedEmployee()
        {
            // Arrange
            var employee = new BankEmployees
            {
                EmployeeID = 1,
                Name = "John Doe",
                Email = "john.doe@example.com",
                Position = "Manager",
                Phone = "1234567890"
            };

           
            await _context.BankEmployees.AddAsync(employee);
            await _context.SaveChangesAsync();

           
            employee.Name = "Updated Name";
            var result = await _service.UpdateEmployee(employee);

           
            var updatedEmployee = await _context.BankEmployees.FindAsync(employee.EmployeeID);

            // Assert
            Assert.IsNotNull(result);
            
        }

        [Test]
        public async Task GetAllEmployees_ReturnsListOfEmployees()
        {
            // Arrange
            var employees = new List<BankEmployees>
    {
        new BankEmployees { EmployeeID = 1, Name = "V", Email = "e@example.com", Position = "Manager", Phone = "1234567890" },
        new BankEmployees { EmployeeID = 2, Name = "V", Email = "e@example.com", Position = "Assistant", Phone = "0987654321" }
    };

            foreach (var emp in employees)
            {
                await _context.BankEmployees.AddAsync(emp);
            }
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetAllEmployees();

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetEmployee_InvalidEmployeeId_ThrowsEmployeeNotFoundException()
        {
            // Arrange
            var invalidEmployeeId = 999;

            // Act & Assert
            var exception = Assert.ThrowsAsync<EmployeeNotFoundException>(() => _service.GetEmployee(invalidEmployeeId));

            // Assert
            Assert.That(exception.Message, Is.EqualTo($"Employee with ID {invalidEmployeeId} not found."));
        }

        [Test]
        public void DeactivateEmployee_InvalidEmployeeId_ThrowsEmployeeNotFoundException()
        {
            // Arrange
            var invalidEmployeeId = 999;

            // Act & Assert
            var exception = Assert.ThrowsAsync<EmployeeNotFoundException>(() => _service.DeactivateEmployee(invalidEmployeeId));

            // Assert
            Assert.That(exception.Message, Is.EqualTo($"Employee with ID {invalidEmployeeId} not found."));
        }

        [Test]
        public void ActivateEmployee_InvalidEmployeeId_ThrowsEmployeeNotFoundException()
        {
            // Arrange
            var invalidEmployeeId = 999;

            // Act & Assert
            var exception = Assert.ThrowsAsync<EmployeeNotFoundException>(() => _service.ActivateEmployee(invalidEmployeeId));

            // Assert
            Assert.That(exception.Message, Is.EqualTo($"Employee with ID {invalidEmployeeId} not found."));
        }



    }
}
