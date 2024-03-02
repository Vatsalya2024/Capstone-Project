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
    public class AdminAvailableLoansServiceTests
    {
        private AdminAvailableLoansService _adminAvailableLoansService;
        private Mock<IRepository<int, AvailableLoans>> _availableLoansRepositoryMock;
        private Mock<ILogger<AdminAvailableLoansService>> _loggerMock;

        [SetUp]
        public void SetUp()
        {
            _availableLoansRepositoryMock = new Mock<IRepository<int, AvailableLoans>>();
            _loggerMock = new Mock<ILogger<AdminAvailableLoansService>>();

            _adminAvailableLoansService = new AdminAvailableLoansService(
                _availableLoansRepositoryMock.Object,
                _loggerMock.Object);
        }

        [Test]
        public async Task AddLoan_ValidLoan_ReturnsAddedLoan()
        {
            // Arrange
            var loanToAdd = new AvailableLoans { LoanID = 1, LoanAmount = 1000, Tenure = 12 };

            _availableLoansRepositoryMock.Setup(r => r.Add(It.IsAny<AvailableLoans>())).ReturnsAsync(loanToAdd);

            // Act
            var result = await _adminAvailableLoansService.AddLoan(loanToAdd);

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
