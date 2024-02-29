using System;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Capstone_ProjectTest
{
    [TestFixture]
    public class AdminServiceTest
    {
        private Mock<IRepository<int, Admin>> _mockAdminRepository;
        private Mock<ILogger<AdminService>> _mockLogger;

        [SetUp]
        public void Setup()
        {
            _mockAdminRepository = new Mock<IRepository<int, Admin>>();
            _mockLogger = new Mock<ILogger<AdminService>>();
        }

        [Test]
        public async Task GetAdmin()
        {
            // Arrange
            var adminId = 1;
            var admin = new Admin { AdminID = adminId, Name = "V" };
            _mockAdminRepository.Setup(repo => repo.Get(adminId)).ReturnsAsync(admin);

            var service = new AdminService(_mockAdminRepository.Object, _mockLogger.Object);

            // Act
            var result = await service.GetAdmin(adminId);

            // Assert
            Assert.That(result.AdminID, Is.EqualTo(adminId));
            
        }

        

        [Test]
        public async Task UpdateAdminName()
        {
            // Arrange
            var adminId = 1;
            var updateAdminNameDTO = new UpdateBankAdminNameDTO { AdminID = adminId, Name = "V" };
            var existingAdmin = new Admin { AdminID = adminId, Name = "B" };
            _mockAdminRepository.Setup(repo => repo.Get(adminId)).ReturnsAsync(existingAdmin);
            _mockAdminRepository.Setup(repo => repo.Update(existingAdmin)).ReturnsAsync(existingAdmin);

            var service = new AdminService(_mockAdminRepository.Object, _mockLogger.Object);

            // Act
            var result = await service.UpdateAdminName(updateAdminNameDTO);

            // Assert
            Assert.That(result.AdminID, Is.EqualTo(adminId));
           
        }

        [Test]
        public async Task GetAllAdmin()
        {
            // Arrange
            var admins = new List<Admin>
            {
                new Admin { AdminID = 1, Name = "Admin1" },
                new Admin { AdminID = 2, Name = "Admin2" },
                new Admin { AdminID = 3, Name = "Admin3" }
            };
            _mockAdminRepository.Setup(repo => repo.GetAll()).ReturnsAsync(admins);

            var service = new AdminService(_mockAdminRepository.Object, _mockLogger.Object);

            // Act
            var result = await service.GetAllAdmin();

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task DeleteAdmin_ValidKey_DeletesAdmin()
        {
            // Arrange
            int adminIdToDelete = 1;
            var adminRepositoryMock = new Mock<IRepository<int, Admin>>();
            adminRepositoryMock.Setup(repo => repo.Delete(adminIdToDelete))
                                .ReturnsAsync(new Admin { AdminID = adminIdToDelete, Name = "Deleted Admin" });

            var loggerMock = new Mock<ILogger<AdminService>>();

            var adminService = new AdminService(adminRepositoryMock.Object, loggerMock.Object);

            // Act
            var deletedAdmin = await adminService.DeleteAdmin(adminIdToDelete);

            // Assert
            Assert.IsNotNull(deletedAdmin);
        }



    }
}

