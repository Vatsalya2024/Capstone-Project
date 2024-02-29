using System;
using Capstone_Project.Context;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Repositories;
using Capstone_Project.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Capstone_ProjectTest
{
    public class BanksServiceTest
    {
        MavericksBankContext context;
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<MavericksBankContext>().UseInMemoryDatabase("dummyDatabase").Options;
            context = new MavericksBankContext(options);
        }

        [Test, Order(1)]
        public void GetAllBanksExceptionTest()
        {
            // Arrange
            var mockBanksRepositoryLogger = new Mock<ILogger<BanksRepository>>();
            var mockBanksServiceLogger = new Mock<ILogger<BanksService>>();
            IRepository<int, Banks> banksRepository = new BanksRepository(context, mockBanksRepositoryLogger.Object);
            IBanksAdminService banksService = new BanksService(banksRepository, mockBanksServiceLogger.Object);

            // Act and Assert
            Assert.ThrowsAsync<NoBanksFoundException>(async () => await banksService.GetAllBanks());
        }




        [Test, Order(2)]
        public async Task GetAllBanksTest()
        {
            // Arrange
            var mockBanksRepositoryLogger = new Mock<ILogger<BanksRepository>>();
            var mockBanksServiceLogger = new Mock<ILogger<BanksService>>();
            IRepository<int, Banks> banksRepository = new BanksRepository(context, mockBanksRepositoryLogger.Object);
            IBanksAdminService banksService = new BanksService(banksRepository, mockBanksServiceLogger.Object);


            await banksService.AddBank(new Banks(1, "Bank X"));

            // Action
            var allBanks = await banksService.GetAllBanks();

            // Assert
            Assert.That(allBanks.Count, Is.Not.EqualTo(0));
        }
        [Test, Order(3)]
        public async Task AddBankTest()
        {
            // Arrange
            var mockBanksRepository = new Mock<IRepository<int, Banks>>();
            var mockLogger = new Mock<ILogger<BanksService>>();


            IBanksAdminService banksService = new BanksService(mockBanksRepository.Object, mockLogger.Object);


            var bankToAdd = new Banks(1, "Test Bank"); // Provide bankID and bankName


            mockBanksRepository.Setup(repo => repo.Add(It.IsAny<Banks>())).ReturnsAsync(bankToAdd);

            // Act
            var addedBank = await banksService.AddBank(bankToAdd);

            // Assert
            Assert.That(addedBank, Is.EqualTo(bankToAdd));
        }
        [Test, Order(4)]
        public async Task DeleteBankTest()
        {
            // Arrange
            var mockBanksRepository = new Mock<IRepository<int, Banks>>();
            var mockLogger = new Mock<ILogger<BanksService>>();


            IBanksAdminService banksService = new BanksService(mockBanksRepository.Object, mockLogger.Object);


            int bankKeyToDelete = 1;


            var deletedBank = new Banks(1, "Test Bank"); // Provide bankID and bankName


            mockBanksRepository.Setup(repo => repo.Delete(bankKeyToDelete)).ReturnsAsync(deletedBank);


            var result = await banksService.DeleteBank(bankKeyToDelete);


            Assert.That(result, Is.EqualTo(deletedBank));
        }

        [Test, Order(5)]
        public async Task GetExistingBankTest()
        {
            // Arrange
            var mockBanksRepository = new Mock<IRepository<int, Banks>>();
            var mockLogger = new Mock<ILogger<BanksService>>();

            IBanksAdminService banksService = new BanksService(mockBanksRepository.Object, mockLogger.Object);


            int bankKeyToRetrieve = 1;

            var retrievedBank = new Banks(1, "Test Bank");


            mockBanksRepository.Setup(repo => repo.Get(bankKeyToRetrieve)).ReturnsAsync(retrievedBank);

            // Act
            var result = await banksService.GetBank(bankKeyToRetrieve);

            // Assert
            Assert.That(result, Is.EqualTo(retrievedBank));
        }

        [Test, Order(6)]
        public async Task UpdateBankNameTest()
        {
            // Arrange
            var mockBanksRepository = new Mock<IRepository<int, Banks>>();
            var mockLogger = new Mock<ILogger<BanksService>>();


            IBanksAdminService banksService = new BanksService(mockBanksRepository.Object, mockLogger.Object);


            int bankIdToUpdate = 1;
            var updateBankNameDto = new UpdateBankNameDTO
            {
                BankID = bankIdToUpdate,
                BankName = "Updated Bank Name"
            };


            var retrievedBank = new Banks(1, "Test Bank");


            mockBanksRepository.Setup(repo => repo.Get(bankIdToUpdate)).ReturnsAsync(retrievedBank);


            mockBanksRepository.Setup(repo => repo.Update(It.IsAny<Banks>())).ReturnsAsync((Banks updatedBank) => updatedBank);


            var updatedBank = await banksService.UpdateBankName(updateBankNameDto);


            Assert.That(updatedBank.BankName, Is.EqualTo(updateBankNameDto.BankName));
        }





    }
}

