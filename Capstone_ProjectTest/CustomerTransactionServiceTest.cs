using Capstone_Project.Exceptions;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone_Project.Tests
{
    [TestFixture]
    public class CustomerTransactionServiceTests
    {
        private CustomerTransactionService _service;
        private Mock<IRepository<int, Transactions>> _transactionsRepositoryMock;
        private Mock<IRepository<long, Accounts>> _accountsRepositoryMock;
        private Mock<ILogger<CustomerTransactionService>> _loggerMock;

        [SetUp]
        public void SetUp()
        {
            _transactionsRepositoryMock = new Mock<IRepository<int, Transactions>>();
            _accountsRepositoryMock = new Mock<IRepository<long, Accounts>>();
            _loggerMock = new Mock<ILogger<CustomerTransactionService>>();

            _service = new CustomerTransactionService(
                _loggerMock.Object,
                _transactionsRepositoryMock.Object,
                _accountsRepositoryMock.Object);
        }

        [Test]
        public async Task Deposit_ValidDepositDTO_ReturnsSuccessMessage()
        {
            // Arrange
            var depositDTO = new DepositDTO
            {
                AccountNumber = 123456789,
                Amount = 100
            };
            var account = new Accounts { AccountNumber = depositDTO.AccountNumber, CustomerID = 1, Status = "Active", Balance = 0 };
            _accountsRepositoryMock.Setup(r => r.Get(depositDTO.AccountNumber)).ReturnsAsync(account);

            // Act
            var result = await _service.Deposit(1, depositDTO);

            // Assert
            Assert.That(result, Is.EqualTo("Deposit successful."));
        }

        [Test]
        public async Task Withdraw_ValidWithdrawalDTO_ReturnsSuccessMessage()
        {
            // Arrange
            var withdrawalDTO = new WithdrawalDTO
            {
                AccountNumber = 123456789,
                Amount = 50
            };
            var account = new Accounts { AccountNumber = withdrawalDTO.AccountNumber, CustomerID = 1, Status = "Active", Balance = 100 };
            _accountsRepositoryMock.Setup(r => r.Get(withdrawalDTO.AccountNumber)).ReturnsAsync(account);

            // Act
            var result = await _service.Withdraw(1, withdrawalDTO);

            // Assert
            Assert.That(result, Is.EqualTo("Withdrawal successful."));
        }

        [Test]
        public async Task Transfer_ValidTransferDTO_ReturnsSuccessMessage()
        {
            // Arrange
            var transferDTO = new TransferDTO
            {
                SourceAccountNumber = 123456789,
                DestinationAccountNumber = 987654321,
                Amount = 50
            };
            var sourceAccount = new Accounts { AccountNumber = transferDTO.SourceAccountNumber, CustomerID = 1, Status = "Active", Balance = 100 };
            var destinationAccount = new Accounts { AccountNumber = transferDTO.DestinationAccountNumber, Status = "Active", Balance = 0 };
            _accountsRepositoryMock.Setup(r => r.Get(transferDTO.SourceAccountNumber)).ReturnsAsync(sourceAccount);
            _accountsRepositoryMock.Setup(r => r.Get(transferDTO.DestinationAccountNumber)).ReturnsAsync(destinationAccount);

            // Act
            var result = await _service.Transfer(1, transferDTO);

            // Assert
            Assert.That(result, Is.EqualTo("Transfer successful."));
        }

        [Test]
        public async Task GetLast10Transactions_ValidAccountNumber_ReturnsTransactions()
        {
            // Arrange
            long accountNumber = 123456789;
            var transactions = new List<Transactions>
            {
                new Transactions { SourceAccountNumber = accountNumber, TransactionDate = DateTime.Now }
            };
            _transactionsRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(transactions);

            // Act
            var result = await _service.GetLast10Transactions(accountNumber);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetLastMonthTransactions_ValidAccountNumber_ReturnsTransactions()
        {
            // Arrange
            long accountNumber = 123456789;
            var transactions = new List<Transactions>
            {
                new Transactions { SourceAccountNumber = accountNumber, TransactionDate = DateTime.Now.AddMonths(-1) }
            };
            _transactionsRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(transactions);

            // Act
            var result = await _service.GetLastMonthTransactions(accountNumber);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetTransactionsBetweenDates_ValidAccountNumber_ReturnsTransactions()
        {
            // Arrange
            long accountNumber = 123456789;
            DateTime startDate = DateTime.Now.AddDays(-10);
            DateTime endDate = DateTime.Now;
            var transactions = new List<Transactions>
            {
                new Transactions { SourceAccountNumber = accountNumber, TransactionDate = DateTime.Now.AddDays(-5) }
            };
            _transactionsRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(transactions);

            // Act
            var result = await _service.GetTransactionsBetweenDates(accountNumber, startDate, endDate);

            // Assert
            Assert.IsNotNull(result);
        }
        [Test]
        public void GetLastMonthTransactions_NoTransactionsFound_ThrowsNoTransactionsException()
        {
            // Arrange
            long accountNumber = 123;
            _transactionsRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Transactions>());

            // Act and Assert
            Assert.ThrowsAsync<NoTransactionsException>(async () => await _service.GetLastMonthTransactions(accountNumber));
        }
        [Test]
        public void GetTransactionsBetweenDates_NoTransactionsFound_ThrowsNoTransactionsException()
        {
            // Arrange
            long accountNumber = 123;
            var startDate = DateTime.Now.AddMonths(-1);
            var endDate = DateTime.Now;
            _transactionsRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Transactions>());

            // Act and Assert
            Assert.ThrowsAsync<NoTransactionsException>(async () => await _service.GetTransactionsBetweenDates(accountNumber, startDate, endDate));
        }

        [Test]
        public async Task GetTransactionsBetweenDates_ValidData_ReturnsFilteredTransactions()
        {
            // Arrange
            long accountNumber = 123;
            var startDate = DateTime.Now.AddMonths(-1);
            var endDate = DateTime.Now;
            var transactions = new List<Transactions>
    {
        new Transactions { TransactionID = 1, SourceAccountNumber = accountNumber, TransactionDate = DateTime.Now.AddDays(-5).Date },
        new Transactions { TransactionID = 2, SourceAccountNumber = accountNumber, TransactionDate = DateTime.Now.AddDays(-10).Date },
        new Transactions { TransactionID = 3, SourceAccountNumber = accountNumber, TransactionDate = DateTime.Now.AddDays(-15).Date }
    };
            _transactionsRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(transactions);

            // Act
            var result = await _service.GetTransactionsBetweenDates(accountNumber, startDate, endDate);

            // Assert
            CollectionAssert.AreEqual(transactions, result);
        }

        [Test]
        public async Task GetLast10Transactions_ValidAccountNumber_ReturnsLast10Transactions()
        {
            // Arrange
            long accountNumber = 123;
            var transactions = new List<Transactions>
        {
            new Transactions { TransactionID = 1, SourceAccountNumber = accountNumber, TransactionDate = DateTime.Now.AddDays(-1).Date },
            new Transactions { TransactionID = 2, SourceAccountNumber = accountNumber, TransactionDate = DateTime.Now.AddDays(-2).Date },

        };
            _transactionsRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(transactions);

            // Act
            var result = await _service.GetLast10Transactions(accountNumber);

            // Assert
            CollectionAssert.AreEqual(transactions.Take(10).OrderByDescending(t => t.TransactionDate), result);
        }

        [Test]
        public void GetLast10Transactions_NoTransactionsFound_ThrowsNoTransactionsException()
        {
            // Arrange
            long accountNumber = 123;
            _transactionsRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Transactions>());

            // Act and Assert
            Assert.ThrowsAsync<NoTransactionsException>(async () => await _service.GetLast10Transactions(accountNumber));
        }

        [Test]
        public async Task GetLast10Transactions_LessThan10Transactions_ReturnsAllTransactions()
        {
            // Arrange
            long accountNumber = 123;
            var transactions = new List<Transactions>
        {
            new Transactions { TransactionID = 1, SourceAccountNumber = accountNumber, TransactionDate = DateTime.Now.AddDays(-1).Date },
            
        };
            _transactionsRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(transactions);

            // Act
            var result = await _service.GetLast10Transactions(accountNumber);

            // Assert
            CollectionAssert.AreEqual(transactions.OrderByDescending(t => t.TransactionDate), result);
        }
    }
}
