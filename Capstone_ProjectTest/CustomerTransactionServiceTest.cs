using System;
using System.Threading.Tasks;
using Capstone_Project.Interfaces;
using Capstone_Project.Mappers;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Capstone_ProjectTest
{
    public class CustomerTransactionServiceTest
    {
        private CustomerTransactionService _transactionService;
        private Mock<IRepository<long, Accounts>> _mockAccountsRepository;
        private Mock<IRepository<int, Transactions>> _mockTransactionsRepository;
        private Mock<ILogger<CustomerTransactionService>> _mockLogger;

        [SetUp]
        public void Setup()
        {
            _mockAccountsRepository = new Mock<IRepository<long, Accounts>>();
            _mockTransactionsRepository = new Mock<IRepository<int, Transactions>>();
            _mockLogger = new Mock<ILogger<CustomerTransactionService>>();

            _transactionService = new CustomerTransactionService(
                _mockLogger.Object,
                _mockTransactionsRepository.Object,
                _mockAccountsRepository.Object,
                new TransactionMapper());
        }

        [Test]
        public async Task Deposit_Successful()
        {
            // Arrange
            var depositDTO = new DepositDTO
            {
                AccountNumber = 123456789,
                Amount = 100
            };

            var account = new Accounts
            {
                AccountNumber = depositDTO.AccountNumber,
                Balance = 0,
                Status = "Active"
            };

            _mockAccountsRepository.Setup(repo => repo.Get(depositDTO.AccountNumber)).ReturnsAsync(account);

            // Act
            var result = await _transactionService.Deposit(depositDTO);

            // Assert
            Assert.AreEqual("Deposit successful.", result);
            Assert.AreEqual(100, account.Balance);
        }

        [Test]
        public async Task Withdraw_From_Active_Account_With_Sufficient_Balance()
        {
            // Arrange
            long accountNumber = 123456789;
            double initialBalance = 1000;
            double withdrawalAmount = 500;

            var account = new Accounts
            {
                AccountNumber = accountNumber,
                Status = "Active",
                Balance = initialBalance
            };

            _mockAccountsRepository.Setup(repo => repo.Get(accountNumber)).ReturnsAsync(account);

            var withdrawalDTO = new WithdrawalDTO
            {
                AccountNumber = accountNumber,
                Amount = withdrawalAmount
            };

            // Act
            var result = await _transactionService.Withdraw(withdrawalDTO);

            // Assert
            Assert.AreEqual("Withdrawal successful.", result);
            Assert.AreEqual(initialBalance - withdrawalAmount, account.Balance);
        }

        [Test]
        public async Task Transfer()
        {
            // Arrange
            long sourceAccountNumber = 123456789;
            long destinationAccountNumber = 987654321;
            double initialSourceBalance = 1000;
            double transferAmount = 500;

            var sourceAccount = new Accounts
            {
                AccountNumber = sourceAccountNumber,
                Status = "Active",
                Balance = initialSourceBalance
            };

            _mockAccountsRepository.Setup(repo => repo.Get(sourceAccountNumber)).ReturnsAsync(sourceAccount);

            var transferDTO = new TransferDTO
            {
                SourceAccountNumber = sourceAccountNumber,
                DestinationAccountNumber = destinationAccountNumber,
                Amount = transferAmount
            };

            // Act
            var result = await _transactionService.Transfer(transferDTO);

            // Assert
            Assert.AreEqual("Transfer successful.", result);
            Assert.AreEqual(initialSourceBalance - transferAmount, sourceAccount.Balance);
        }
    }
}
