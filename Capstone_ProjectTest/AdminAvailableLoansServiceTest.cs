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
            Assert.AreEqual(loanToAdd, addedLoan);
        }

        [Test]
        public async Task DeleteLoan_ExistingLoanId_ShouldReturnDeletedLoan()
        {
            // Arrange
            int loanIdToDelete = 1;
            var loanToDelete = new AvailableLoans { LoanID = loanIdToDelete, LoanAmount = 20000, LoanType = "Home Loan", Interest = 4.5, Tenure = 24, Purpose = "Education", Status = "Active" };
            _mockRepository.Setup(repo => repo.Delete(loanIdToDelete)).ReturnsAsync(loanToDelete);

            // Act
            var deletedLoan = await _service.DeleteLoan(loanIdToDelete);

            // Assert
            Assert.AreEqual(loanToDelete, deletedLoan);
        }

        [Test]
        public async Task UpdateLoan_ValidLoan_ShouldReturnUpdatedLoan()
        {
            // Arrange
            var loanToUpdate = new AvailableLoans { LoanID = 1, LoanAmount = 30000, LoanType = "Car Loan", Interest = 6, Tenure = 36, Purpose = "Vehicle Purchase", Status = "Active" };
            _mockRepository.Setup(repo => repo.Update(loanToUpdate)).ReturnsAsync(loanToUpdate);

            // Act
            var updatedLoan = await _service.UpdateLoan(loanToUpdate);

            // Assert
            Assert.AreEqual(loanToUpdate, updatedLoan);
        }
    }
}

