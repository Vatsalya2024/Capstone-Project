using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone_Project.Context;
using Capstone_Project.Controllers;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Capstone_ProjectTest
{
    [TestFixture]
    public class BankEmployeeTransactionServiceTest
    {
        private Mock<IRepository<int, Transactions>> _mockTransactionsRepository;
        private Mock<ILogger<CustomerTransactionService>> _mockLogger;

        [SetUp]
        public void Setup()
        {
            _mockTransactionsRepository = new Mock<IRepository<int, Transactions>>();
            _mockLogger = new Mock<ILogger<CustomerTransactionService>>();
        }

        [Test]
        public async Task GetAllTransactions()
        {
            // Arrange
            var service = new BankEmployeeTransactionService(_mockTransactionsRepository.Object, _mockLogger.Object);
            var expectedTransactions = new List<Transactions> { new Transactions(), new Transactions() };
            _mockTransactionsRepository.Setup(repo => repo.GetAll()).ReturnsAsync(expectedTransactions);

            // Act
            var result = await service.GetAllTransactions();

            // Assert
            Assert.That(result, Is.EqualTo(expectedTransactions));
        }
        [Test]
        public async Task GetTransactionsByAccountNumber()
        {
            // Arrange
            var accountNumber = 1234567890L;
            var expectedTransactions = new List<Transactions>
            {
                new Transactions { SourceAccountNumber = accountNumber, Amount = 100 },
                new Transactions { DestinationAccountNumber = accountNumber, Amount = 200 }
            };
            _mockTransactionsRepository.Setup(repo => repo.GetAll()).ReturnsAsync(expectedTransactions);

            var service = new BankEmployeeTransactionService(_mockTransactionsRepository.Object, _mockLogger.Object);

            // Act
            var result = await service.GetTransactionsByAccountNumber(accountNumber);

            // Assert
            
            Assert.IsTrue(result?.All(t => t.SourceAccountNumber == accountNumber || t.DestinationAccountNumber == accountNumber));
        }
        [Test]
        public async Task GetTotalInboundTransactions()
        {
            // Arrange
            var accountNumber = 1234567890L;
            var transactions = new List<Transactions>
            {
                new Transactions { SourceAccountNumber = 9876543210L, Amount = 100, TransactionType = "Credit" },
                new Transactions { SourceAccountNumber = accountNumber, Amount = 200, TransactionType = "Credit" },
                new Transactions { SourceAccountNumber = 9876543210L, Amount = 300, TransactionType = "Debit" },
                new Transactions { SourceAccountNumber = accountNumber, Amount = 400, TransactionType = "Credit" }
            };
            _mockTransactionsRepository.Setup(repo => repo.GetAll()).ReturnsAsync(transactions);

            var service = new BankEmployeeTransactionService(_mockTransactionsRepository.Object, _mockLogger.Object);

            // Act
            var result = await service.GetTotalInboundTransactions(accountNumber);

            // Assert
            Assert.That(result, Is.EqualTo(600));
        }
        [Test]
        public async Task GetTotalOutboundTransactions()
        {
            // Arrange
            var accountNumber = 1234567890L;
            var transactions = new List<Transactions>
            {
                new Transactions { SourceAccountNumber = accountNumber, Amount = 100, TransactionType = "Debit" },
                new Transactions { SourceAccountNumber = 9876543210L, Amount = 200, TransactionType = "Credit" },
                new Transactions { SourceAccountNumber = accountNumber, Amount = 300, TransactionType = "Debit" },
                new Transactions { SourceAccountNumber = 9876543210L, Amount = 400, TransactionType = "Debit" }
            };
            _mockTransactionsRepository.Setup(repo => repo.GetAll()).ReturnsAsync(transactions);

            var service = new BankEmployeeTransactionService(_mockTransactionsRepository.Object, _mockLogger.Object);

            // Act
            var result = await service.GetTotalOutboundTransactions(accountNumber);

            // Assert
            Assert.That(result, Is.EqualTo(400));
        }

        [Test]
        public async Task GetAllTransactions_RepositoryReturnsNull_ReturnsNull()
        {
            // Arrange
            var service = new BankEmployeeTransactionService(_mockTransactionsRepository.Object, _mockLogger.Object);
            _mockTransactionsRepository.Setup(repo => repo.GetAll()).ReturnsAsync((List<Transactions>?)null);

            // Act
            var result = await service.GetAllTransactions();

            // Assert
            Assert.IsNull(result, "Result should be null");
        }
        [Test]
        public void GetTransactionsByAccountNumber_NoTransactionsFound_ThrowsBankTransactionServiceException()
        {
            // Arrange
            var accountNumber = 1234567890L;
            _mockTransactionsRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Transactions>());

            var service = new BankEmployeeTransactionService(_mockTransactionsRepository.Object, _mockLogger.Object);

            // Act & Assert
            Assert.ThrowsAsync<BankTransactionServiceException>(() => service.GetTransactionsByAccountNumber(accountNumber));
        }

        [Test]
        public void GetTotalInboundTransactions_NoTransactionsFound_ThrowsNoAccountsFoundException()
        {
            // Arrange
            var accountNumber = 1234567890L;
            _mockTransactionsRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Transactions>());

            var service = new BankEmployeeTransactionService(_mockTransactionsRepository.Object, _mockLogger.Object);

            // Act & Assert
            Assert.ThrowsAsync<NoAccountsFoundException>(() => service.GetTotalInboundTransactions(accountNumber));
        }
        [Test]
        public void GetTotalOutboundTransactions_NoTransactionsFound_ThrowsBankTransactionServiceException()
        {
            // Arrange
            var accountNumber = 1234567890L;
            _mockTransactionsRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Transactions>());

            var service = new BankEmployeeTransactionService(_mockTransactionsRepository.Object, _mockLogger.Object);

            // Act & Assert
            Assert.ThrowsAsync<NoAccountsFoundException>(() => service.GetTotalOutboundTransactions(accountNumber));
        }



    }

}
