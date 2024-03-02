using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Capstone_Project.Tests.Services
{
    [TestFixture]
    public class AvailableLoansUserServiceTests
    {
        private AvailableLoansUserService _availableLoansUserService;
        private Mock<IRepository<int, AvailableLoans>> _availableLoansRepositoryMock;
        private Mock<ILogger<AvailableLoansUserService>> _loggerMock;

        [SetUp]
        public void SetUp()
        {
            _availableLoansRepositoryMock = new Mock<IRepository<int, AvailableLoans>>();
            _loggerMock = new Mock<ILogger<AvailableLoansUserService>>();

            _availableLoansUserService = new AvailableLoansUserService(
                _availableLoansRepositoryMock.Object,
                _loggerMock.Object);
        }

        [Test]
        public async Task GetAllLoans_WhenCalled_ReturnsListOfLoans()
        {
            // Arrange
            var expectedLoans = new List<AvailableLoans>
            {
                new AvailableLoans { LoanID = 1, LoanAmount = 1000 },
                new AvailableLoans { LoanID = 2, LoanAmount = 2000 }
            };

            _availableLoansRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(expectedLoans);

            // Act
            var result = await _availableLoansUserService.GetAllLoans();

            // Assert
            Assert.IsNotNull(result);
        }
    }
}