using System;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Capstone_ProjectTest
{
    public class AdminAvailableLoansServiceTest
    {
        private AdminAvailableLoansService _service;
        private Mock<IRepository<int, AvailableLoans>> _mockRepository;
        private Mock<ILogger<AdminAvailableLoansService>> _mockLogger;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IRepository<int, AvailableLoans>>();
            _mockLogger = new Mock<ILogger<AdminAvailableLoansService>>();
            _service = new AdminAvailableLoansService(_mockRepository.Object);
        }

        [Test]
        public async Task AddLoan_ValidLoan_ShouldReturnAddedLoan()
        {
            // Arrange
            var loanToAdd = new AvailableLoans { LoanID = 1, LoanAmount = 10000, LoanType = "Personal Loan", Interest = 5, Tenure = 12, Purpose = "Home Renovation", Status = "Active" };
            _mockRepository.Setup(repo => repo.Add(loanToAdd)).ReturnsAsync(loanToAdd);

            // Act
            var addedLoan = await _service.AddLoan(loanToAdd);

            // Assert
            Assert.That(addedLoan, Is.EqualTo(loanToAdd));
        }

    
    }
}

