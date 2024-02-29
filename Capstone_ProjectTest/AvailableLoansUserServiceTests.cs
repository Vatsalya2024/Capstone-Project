using System;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Services;
using Moq;

namespace Capstone_ProjectTest
{
    [TestFixture]
    public class AvailableLoansUserServiceTests
    {
        private Mock<IRepository<int, AvailableLoans>> _availableLoansRepositoryMock;
        private AvailableLoansUserService _availableLoansUserService;

        [SetUp]
        public void Setup()
        {
            _availableLoansRepositoryMock = new Mock<IRepository<int, AvailableLoans>>();
            _availableLoansUserService = new AvailableLoansUserService(_availableLoansRepositoryMock.Object);
        }

        [Test]
        public async Task GetAllLoans()
        {
            // Arrange
            var expectedLoans = new List<AvailableLoans>
            {
                new AvailableLoans { LoanID = 1, LoanType = "Loan 1", LoanAmount = 10000 },
                new AvailableLoans { LoanID = 2, LoanType = "Loan 2", LoanAmount = 20000 }
            };

            _availableLoansRepositoryMock.Setup(repo => repo.GetAll())
                .ReturnsAsync(expectedLoans);

            // Act
            var result = await _availableLoansUserService.GetAllLoans();

            // Assert
            Assert.IsNotNull(result);

        }

       
    }
}

