using NUnit.Framework;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Capstone_ProjectTest
{
    [TestFixture]
    public class BankEmployeeAccountServiceTests
    {
        [Test]
        public async Task GetCustomers_ReturnsSingleCustomer()
        {
            // Arrange
            var customerId = 1;
            var expectedCustomer = new Customers { CustomerID = customerId, Name = "V" };

            var customerRepositoryMock = new Mock<IRepository<int, Customers>>();
            customerRepositoryMock.Setup(repo => repo.Get(customerId)).ReturnsAsync(expectedCustomer);

            var loggerMock = new Mock<ILogger<BankEmployeeAccountService>>();

            var service = new BankEmployeeAccountService(
                Mock.Of<IRepository<long, Accounts>>(),
                customerRepositoryMock.Object,
                loggerMock.Object);

            // Act
            var result = await service.GetCustomers(customerId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedCustomer));
        }

        [Test]
        public async Task GetCustomersList_ReturnsListOfCustomers()
        {
            // Arrange
            var expectedCustomers = new List<Customers>
            {
                new Customers { CustomerID = 1, Name = "V" },
                new Customers { CustomerID = 2, Name = "B" }
            };

            var customerRepositoryMock = new Mock<IRepository<int, Customers>>();
            customerRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(expectedCustomers);

            var loggerMock = new Mock<ILogger<BankEmployeeAccountService>>();

            var service = new BankEmployeeAccountService(
                Mock.Of<IRepository<long, Accounts>>(),
                customerRepositoryMock.Object,
                loggerMock.Object);

            // Act
            var result = await service.GetCustomersListasync();

            // Assert
            Assert.That(result, Is.EqualTo(expectedCustomers));
        }

        [Test]
        public async Task ApproveAccountCreation_ReturnsTrue()
        {
            // Arrange
            var accountNumber = 123456789;
            var pendingAccount = new Accounts { AccountNumber = accountNumber, Status = "Pending" };

            var accountsRepositoryMock = new Mock<IRepository<long, Accounts>>();
            accountsRepositoryMock.Setup(repo => repo.Get(accountNumber)).ReturnsAsync(pendingAccount);
            accountsRepositoryMock.Setup(repo => repo.Update(pendingAccount)).ReturnsAsync(pendingAccount);

            var loggerMock = new Mock<ILogger<BankEmployeeAccountService>>();

            var service = new BankEmployeeAccountService(
                accountsRepositoryMock.Object,
                Mock.Of<IRepository<int, Customers>>(),
                loggerMock.Object);

            // Act
            var result = await service.ApproveAccountCreation(accountNumber);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ApproveAccountDeletion_ReturnsTrue()
        {
            // Arrange
            var accountNumber = 123456789;
            var pendingDeletionAccount = new Accounts { AccountNumber = accountNumber, Status = "PendingDeletion" };

            var accountsRepositoryMock = new Mock<IRepository<long, Accounts>>();
            accountsRepositoryMock.Setup(repo => repo.Get(accountNumber)).ReturnsAsync(pendingDeletionAccount);
            accountsRepositoryMock.Setup(repo => repo.Delete(accountNumber)).ReturnsAsync(pendingDeletionAccount);

            var loggerMock = new Mock<ILogger<BankEmployeeAccountService>>();

            var service = new BankEmployeeAccountService(
                accountsRepositoryMock.Object,
                Mock.Of<IRepository<int, Customers>>(),
                loggerMock.Object);

            // Act
            var result = await service.ApproveAccountDeletion(accountNumber);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetPendingAccounts_ReturnsListOfAccounts()
        {
            // Arrange
            var pendingAccounts = new List<Accounts>
            {
                new Accounts { AccountNumber = 1, Status = "Pending" },
                new Accounts { AccountNumber = 2, Status = "Pending" }
            };

            var accountsRepositoryMock = new Mock<IRepository<long, Accounts>>();
            accountsRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(pendingAccounts);

            var loggerMock = new Mock<ILogger<BankEmployeeAccountService>>();

            var service = new BankEmployeeAccountService(
                accountsRepositoryMock.Object,
                Mock.Of<IRepository<int, Customers>>(),
                loggerMock.Object);

            // Act
            var result = await service.GetPendingAccounts();

            // Assert
            Assert.That(result, Is.EqualTo(pendingAccounts));
        }

        [Test]
        public async Task GetPendingDeletionAccounts_ReturnsListOfAccounts()
        {
            // Arrange
            var pendingDeletionAccounts = new List<Accounts>
            {
                new Accounts { AccountNumber = 1, Status = "PendingDeletion" },
                new Accounts { AccountNumber = 2, Status = "PendingDeletion" }
            };

            var accountsRepositoryMock = new Mock<IRepository<long, Accounts>>();
            accountsRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(pendingDeletionAccounts);

            var loggerMock = new Mock<ILogger<BankEmployeeAccountService>>();

            var service = new BankEmployeeAccountService(
                accountsRepositoryMock.Object,
                Mock.Of<IRepository<int, Customers>>(),
                loggerMock.Object);

            // Act
            var result = await service.GetPendingDeletionAccounts();

            // Assert
            Assert.That(result, Is.EqualTo(pendingDeletionAccounts));
        }
        [Test]
        public void GetCustomers_InvalidCustomerId_ThrowsNotFoundException()
        {
            // Arrange
            var invalidCustomerId = 999; 

            var customerRepositoryMock = new Mock<IRepository<int, Customers>>();
            customerRepositoryMock.Setup(repo => repo.Get(invalidCustomerId)).ReturnsAsync((Customers?)null);

            var loggerMock = new Mock<ILogger<BankEmployeeAccountService>>();

            var service = new BankEmployeeAccountService(
                Mock.Of<IRepository<long, Accounts>>(),
                customerRepositoryMock.Object,
                loggerMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<NoCustomersFoundException>(async () => await service.GetCustomers(invalidCustomerId));
        }
    }
}
