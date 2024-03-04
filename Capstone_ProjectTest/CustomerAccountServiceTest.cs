using System;
using System.Collections.Generic;
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
    [TestFixture]
    public class CustomerAccountServiceTests
    {
        private Mock<IRepository<long, Accounts>> _mockAccountsRepository;
        private Mock<IRepository<int, Transactions>> _mockTransactionsRepository;
        private Mock<ILogger<CustomerAccountService>> _mockLogger;
        private CustomerAccountService _customerAccountService;

        [SetUp]
        public void Setup()
        {
            _mockAccountsRepository = new Mock<IRepository<long, Accounts>>();
            _mockTransactionsRepository = new Mock<IRepository<int, Transactions>>();
            _mockLogger = new Mock<ILogger<CustomerAccountService>>();
            _customerAccountService = new CustomerAccountService(
                _mockAccountsRepository.Object,
                _mockTransactionsRepository.Object,
                _mockLogger.Object);
        }

        [Test]
        public async Task CloseAccount_ValidAccount_ReturnsSuccessMessage()
        {
            // Arrange
            long accountNumber = 123456789;

            var account = new Accounts
            {
                AccountNumber = accountNumber
            };

            _mockAccountsRepository.Setup(repo => repo.Get(accountNumber)).ReturnsAsync(account);
            _mockAccountsRepository.Setup(repo => repo.Update(It.IsAny<Accounts>())).ReturnsAsync(account);

            // Act
            var result = await _customerAccountService.CloseAccount(accountNumber);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAccountDetails_ValidAccount_ReturnsAccount()
        {
            // Arrange
            long accountNumber = 123456789;
            int customerId = 1;

            var account = new Accounts
            {
                AccountNumber = accountNumber,
                CustomerID = customerId
            };

            _mockAccountsRepository.Setup(repo => repo.Get(accountNumber)).ReturnsAsync(account);

            // Act
            var result = await _customerAccountService.GetAccountDetails(accountNumber, customerId);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAllAccountsByCustomerId_ValidCustomerId_ReturnsCustomerAccounts()
        {
            // Arrange
            int customerId = 1;

            var accounts = new List<Accounts>
            {
                new Accounts { AccountNumber = 123, CustomerID = customerId },
                new Accounts { AccountNumber = 456, CustomerID = customerId }
            };

            _mockAccountsRepository.Setup(repo => repo.GetAll()).ReturnsAsync(accounts);

            // Act
            var result = await _customerAccountService.GetAllAccountsByCustomerId(customerId);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task OpenNewAccount_ValidAccountOpeningDTO_ReturnsAddedAccount()
        {
            // Arrange
            var accountOpeningDTO = new AccountOpeningDTO
            {
                AccountType = "Savings",
                IFSC = "ABC123",
                CustomerID = 1
            };

            var addedAccount = new Accounts
            {
                AccountNumber = 123,
                AccountType = accountOpeningDTO.AccountType,
                IFSC = accountOpeningDTO.IFSC,
                CustomerID = accountOpeningDTO.CustomerID
            };

            _mockAccountsRepository.Setup(repo => repo.Add(It.IsAny<Accounts>())).ReturnsAsync(addedAccount);

            // Act
            var result = await _customerAccountService.OpenNewAccount(accountOpeningDTO);

            // Assert
            Assert.IsNotNull(result);
        }
        [Test]
        public void CloseAccount_AccountNotFound_ThrowsNoAccountsFoundException()
        {
            // Arrange
            long accountNumber = 123456789;
            _mockAccountsRepository.Setup(repo => repo.Get(accountNumber)).ReturnsAsync((Accounts?)null);

            // Act & Assert
            Assert.ThrowsAsync<NoAccountsFoundException>(() => _customerAccountService.CloseAccount(accountNumber));
        }
        [Test]
        public void GetAccountDetails_AccountNotFound_ThrowsNoAccountsFoundException()
        {
            // Arrange
            int customerId = 1;
            long accountNumber = 123456789;
            _mockAccountsRepository.Setup(repo => repo.Get(accountNumber)).ReturnsAsync((Accounts?)null);

            // Act & Assert
            Assert.ThrowsAsync<NoAccountsFoundException>(() => _customerAccountService.GetAccountDetails(accountNumber, customerId));
        }
        [Test]
        public void GetAllAccountsByCustomerId_NoAccountsFound_ThrowsNoAccountsFoundException()
        {
            // Arrange
            int customerId = 1;
            _mockAccountsRepository.Setup(repo => repo.GetAll()).ReturnsAsync((List<Accounts>?)null);

            // Act & Assert
            Assert.ThrowsAsync<NoAccountsFoundException>(() => _customerAccountService.GetAllAccountsByCustomerId(customerId));
        }


    }
}
