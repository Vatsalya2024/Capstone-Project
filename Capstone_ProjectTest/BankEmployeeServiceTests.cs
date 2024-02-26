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
    public class BankEmployeeServiceTests
    {
        [Test]
        public async Task GetAllBankEmployee()
        {
            // Arrange
            var expectedBankEmployees = new List<BankEmployees>
            {
                new BankEmployees { EmployeeID = 1, Name = "V" },
                new BankEmployees { EmployeeID = 2, Name = "B" }
            };

            var bankEmployeeRepositoryMock = new Mock<IRepository<int, BankEmployees>>();
            bankEmployeeRepositoryMock.Setup(repo => repo.GetAll())
                                       .ReturnsAsync(expectedBankEmployees);

            var loggerMock = new Mock<ILogger<BankEmployeeService>>();

            var bankEmployeeService = new BankEmployeeService(bankEmployeeRepositoryMock.Object, loggerMock.Object);

            // Act
            var actualBankEmployees = await bankEmployeeService.GetAllBankEmployee();

            // Assert
            Assert.That(actualBankEmployees, Is.EqualTo(expectedBankEmployees));
        }

        [Test]
        public void GetAllBankEmployee_ThrowsException()
        {
            // Arrange
            var bankEmployeeRepositoryMock = new Mock<IRepository<int, BankEmployees>>();
            bankEmployeeRepositoryMock.Setup(repo => repo.GetAll())
                                       .ReturnsAsync((List<BankEmployees>?)null);

            var loggerMock = new Mock<ILogger<BankEmployeeService>>();

            var bankEmployeeService = new BankEmployeeService(bankEmployeeRepositoryMock.Object, loggerMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<NoBankEmployeesFoundException>(() => bankEmployeeService.GetAllBankEmployee());
        }
        [Test]
        public async Task DeleteBankEmployee()
        {
            // Arrange
            var employeeIdToDelete = 1;
            var deletedEmployee = new BankEmployees { EmployeeID = employeeIdToDelete, Name = "Employee" };

            var bankEmployeeRepositoryMock = new Mock<IRepository<int, BankEmployees>>();
            bankEmployeeRepositoryMock.Setup(repo => repo.Delete(employeeIdToDelete)).ReturnsAsync(deletedEmployee);

            var loggerMock = new Mock<ILogger<BankEmployeeService>>();

            var bankEmployeeService = new BankEmployeeService(bankEmployeeRepositoryMock.Object, loggerMock.Object);

            // Act
            var result = await bankEmployeeService.DeleteBankEmployee(employeeIdToDelete);

            // Assert
            Assert.That(result, Is.EqualTo(deletedEmployee));
        }

        [Test]
        public async Task GetBankEmployee()
        {
            // Arrange
            var employeeIdToGet = 1;
            var expectedEmployee = new BankEmployees { EmployeeID = employeeIdToGet, Name = "Employee" };

            var bankEmployeeRepositoryMock = new Mock<IRepository<int, BankEmployees>>();
            bankEmployeeRepositoryMock.Setup(repo => repo.Get(employeeIdToGet)).ReturnsAsync(expectedEmployee);

            var loggerMock = new Mock<ILogger<BankEmployeeService>>();

            var bankEmployeeService = new BankEmployeeService(bankEmployeeRepositoryMock.Object, loggerMock.Object);

            // Act
            var result = await bankEmployeeService.GetBankEmployee(employeeIdToGet);

            // Assert
            Assert.That(result, Is.EqualTo(expectedEmployee));
        }

        [Test]
        public async Task UpdateBankEmployeeName()
        {
            // Arrange
            var updateDto = new UpdateBankEmployeeNameDTO { EmployeeID = 1, Name = "New Name" };
            var existingEmployee = new BankEmployees { EmployeeID = updateDto.EmployeeID, Name = "Original Name" };
            var updatedEmployee = new BankEmployees { EmployeeID = updateDto.EmployeeID, Name = updateDto.Name };

            var bankEmployeeRepositoryMock = new Mock<IRepository<int, BankEmployees>>();
            bankEmployeeRepositoryMock.Setup(repo => repo.Get(updateDto.EmployeeID)).ReturnsAsync(existingEmployee);
            bankEmployeeRepositoryMock.Setup(repo => repo.Update(existingEmployee)).ReturnsAsync(updatedEmployee);

            var loggerMock = new Mock<ILogger<BankEmployeeService>>();

            var bankEmployeeService = new BankEmployeeService(bankEmployeeRepositoryMock.Object, loggerMock.Object);

            // Act
            var result = await bankEmployeeService.UpdateBankEmployeeName(updateDto);

            // Assert
            Assert.That(result.Name, Is.EqualTo(updateDto.Name));
        }


    }
}

