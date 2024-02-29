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
    public class CustomerBeneficiaryServiceTests
    {
        private CustomerBeneficiaryService _service;
        private Mock<IRepository<int, Beneficiaries>> _beneficiaryRepositoryMock;
        private Mock<IRepository<string, Branches>> _branchesRepositoryMock;
        private Mock<ILogger<CustomerBeneficiaryService>> _loggerMock;
        private Mock<IRepository<int, Customers>> _customerRepositoryMock;
        private Mock<IRepository<int, Transactions>> _transactionRepositoryMock;
        private Mock<IRepository<long, Accounts>> _accountRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            _beneficiaryRepositoryMock = new Mock<IRepository<int, Beneficiaries>>();
            _branchesRepositoryMock = new Mock<IRepository<string, Branches>>();
            _loggerMock = new Mock<ILogger<CustomerBeneficiaryService>>();
            _customerRepositoryMock = new Mock<IRepository<int, Customers>>();
            _transactionRepositoryMock = new Mock<IRepository<int, Transactions>>();
            _accountRepositoryMock = new Mock<IRepository<long, Accounts>>();

            _service = new CustomerBeneficiaryService(
                _beneficiaryRepositoryMock.Object,
                _branchesRepositoryMock.Object,
                _customerRepositoryMock.Object,
                _accountRepositoryMock.Object,
                _transactionRepositoryMock.Object,
                _loggerMock.Object);
        }

        [Test]
        public async Task GetBeneficiariesByCustomerID_ValidCustomerID_ReturnsBeneficiaries()
        {
            // Arrange
            int customerId = 1;
            var beneficiaries = new List<Beneficiaries>
            {
                new Beneficiaries { BeneficiaryID = 1, CustomerID = customerId },
                new Beneficiaries { BeneficiaryID = 2, CustomerID = customerId },
            };
            _beneficiaryRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(beneficiaries);

            // Act
            var result = await _service.GetBeneficiariesByCustomerID(customerId);

            // Assert
            Assert.IsNotNull(result);
            
        }

        [Test]
        public async Task AddBeneficiary_ValidBeneficiaryDTO_ReturnsAddedBeneficiary()
        {
            // Arrange
            var beneficiaryDTO = new BeneficiaryDTO
            {
                BeneficiaryAccountNumber = 1234567890,
                Name = "Test Beneficiary",
                IFSC = "ABC123",
                CustomerID = 1
            };
            _beneficiaryRepositoryMock.Setup(r => r.Add(It.IsAny<Beneficiaries>())).ReturnsAsync(new Beneficiaries());

            // Act
            var result = await _service.AddBeneficiary(beneficiaryDTO);

            // Assert
            Assert.IsNotNull(result);
        }






        [Test]
        public async Task GetIFSCByBranch_ValidBranchName_ReturnsIFSC()
        {
            // Arrange
            string branchName = "TestBranch";
            var branches = new List<Branches>
    {
        new Branches("IFSC123", branchName, 1), 
    };
            _branchesRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(branches);

            // Act
            var result = await _service.GetIFSCByBranch(branchName);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task TransferToBeneficiary_ValidTransferDTO_TransfersSuccessfully()
        {
            // Arrange
            var transferDTO = new BeneficiaryTransferDTO
            {
                SourceAccountNumber = 1234567890,
                BeneficiaryID = 1,
                Amount = 100
            };

            var sourceAccount = new Accounts { AccountNumber = transferDTO.SourceAccountNumber, Status = "Active", Balance = 200 };
            var beneficiary = new Beneficiaries { BeneficiaryID = transferDTO.BeneficiaryID, BeneficiaryAccountNumber = 9876543210, Balance = 0 };

            _accountRepositoryMock.Setup(r => r.Get(transferDTO.SourceAccountNumber)).ReturnsAsync(sourceAccount);
            _beneficiaryRepositoryMock.Setup(r => r.Get(transferDTO.BeneficiaryID)).ReturnsAsync(beneficiary);

            // Act
            var result = await _service.TransferToBeneficiary(transferDTO);

            // Assert
            Assert.NotNull(result);
        }


    }
}
