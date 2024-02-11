using System;
using System.Threading.Tasks;
using Capstone_Project.Controllers;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Capstone_ProjectTest
{
    public class CustomerAccountServiceTest
    {
        private CustomerAccountService _accountService;
        private Mock<IRepository<long, Accounts>> _mockAccountsRepository;
        private Mock<IRepository<int, Transactions>> _mockTransactionsRepository;
        private Mock<ILogger<CustomerAccountService>> _mockLogger;

        [SetUp]
        public void Setup()
        {
            _mockAccountsRepository = new Mock<IRepository<long, Accounts>>();
            _mockTransactionsRepository = new Mock<IRepository<int, Transactions>>();
            _mockLogger = new Mock<ILogger<CustomerAccountService>>();

            _accountService = new CustomerAccountService(
                _mockAccountsRepository.Object,
                _mockTransactionsRepository.Object,
                _mockLogger.Object);
        }

        [Test]
        public async Task CloseAccount_AccountExists_Success()
        {
            // Arrange
            long accountNumber = 123456789;
            var account = new Accounts
            {
                AccountNumber = accountNumber,
                Status = "Active"
            };

            _mockAccountsRepository.Setup(repo => repo.Get(accountNumber)).ReturnsAsync(account);

            // Act
            var result = await _accountService.CloseAccount(accountNumber);

            // Assert
            Assert.AreEqual($"Account with number {accountNumber} is scheduled for deletion.", result);
            Assert.AreEqual("PendingDeletion", account.Status);
        }

        [Test]
        public void CloseAccount_AccountDoesNotExist_ExceptionThrown()
        {
            // Arrange
            long accountNumber = 123456789;
            _mockAccountsRepository.Setup(repo => repo.Get(accountNumber)).ReturnsAsync((Accounts)null);

            // Act & Assert
            Assert.ThrowsAsync<NoAccountsFoundException>(async () => await _accountService.CloseAccount(accountNumber));
        }

        [Test]
        public async Task GetAccountDetails_AccountExists_Success()
        {
            // Arrange
            long accountNumber = 123456789;
            var account = new Accounts
            {
                AccountNumber = accountNumber
            };

            _mockAccountsRepository.Setup(repo => repo.Get(accountNumber)).ReturnsAsync(account);

            // Act
            var result = await _accountService.GetAccountDetails(accountNumber);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(accountNumber, result.AccountNumber);
        }

        [Test]
        public void GetAccountDetails_AccountDoesNotExist_ExceptionThrown()
        {
            // Arrange
            long accountNumber = 123456789;
            _mockAccountsRepository.Setup(repo => repo.Get(accountNumber)).ReturnsAsync((Accounts)null);

            // Act & Assert
            Assert.ThrowsAsync<NoAccountsFoundException>(async () => await _accountService.GetAccountDetails(accountNumber));
        }

    }
}
