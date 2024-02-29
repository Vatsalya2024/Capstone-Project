using System;
using Capstone_Project.Context;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Repositories;
using Capstone_Project.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Capstone_ProjectTest
{
    public class BranchServiceTest
    {
        MavericksBankContext context;
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<MavericksBankContext>().UseInMemoryDatabase("dummyDatabase").Options;
            context = new MavericksBankContext(options);
        }
        [Test, Order(1)]
        public void GetAllBranchesExceptionTest()
        {
            // Arrange
            var mockBranchesRepositoryLogger = new Mock<ILogger<BranchesRepository>>();
            var mockBranchesServiceLogger = new Mock<ILogger<BranchesService>>();
            IRepository<string, Branches> branchesRepository = new BranchesRepository(context, mockBranchesRepositoryLogger.Object);
            IBranchesAdminService branchesService = new BranchesService(branchesRepository, mockBranchesServiceLogger.Object);

            // Act and Assert
            Assert.ThrowsAsync<NoBranchesFoundException>(async () => await branchesService.GetAllBranches());
        }
        [Test, Order(2)]
        public async Task AddBranchTest()
        {
            // Arrange
            var mockBranchesRepository = new Mock<IRepository<string, Branches>>();
            var mockLogger = new Mock<ILogger<BranchesService>>();


            IBranchesAdminService branchesService = new BranchesService(mockBranchesRepository.Object, mockLogger.Object);

            // Create a branch to add
            var branchToAdd = new Branches(
                "Branch Name",
                "Location",
                 123456
  );


            mockBranchesRepository.Setup(repo => repo.Add(branchToAdd)).ReturnsAsync(branchToAdd);

            // Act
            var addedBranch = await branchesService.AddBranch(branchToAdd);

            // Assert
            Assert.That(addedBranch, Is.EqualTo(branchToAdd));
        }

        [Test, Order(3)]
        public void DeleteBranchTest()
        {
            // Arrange
            var mockBranchesRepository = new Mock<IRepository<string, Branches>>();
            var mockLogger = new Mock<ILogger<BranchesService>>();

            IBranchesAdminService branchesService = new BranchesService(mockBranchesRepository.Object, mockLogger.Object);


            string branchKeyToDelete = "123456";


            mockBranchesRepository.Setup(repo => repo.Delete(branchKeyToDelete))
                                   .ThrowsAsync(new NoBranchesFoundException($"Branch IFSC {branchKeyToDelete} not found"));

            // Act and Assert: 
            Assert.ThrowsAsync<NoBranchesFoundException>(async () => await branchesService.DeleteBranch(branchKeyToDelete));
        }
        [Test, Order(4)]
        public async Task GetAllBranchesTest()
        {
            // Arrange
            var mockBranchesRepository = new Mock<IRepository<string, Branches>>();
            var mockLogger = new Mock<ILogger<BranchesService>>();

            IBranchesAdminService branchesService = new BranchesService(mockBranchesRepository.Object, mockLogger.Object);

            var expectedBranches = new List<Branches>
           {
               new Branches("Branch1", "Location1", 123456),
               new Branches("Branch2", "Location2", 789012)

            };


            mockBranchesRepository.Setup(repo => repo.GetAll()).ReturnsAsync(expectedBranches);

            // Act: Get all branches
            var actualBranches = await branchesService.GetAllBranches();

            // Assert: 
            Assert.That(actualBranches.Count, Is.EqualTo(expectedBranches.Count));
            for (int i = 0; i < expectedBranches.Count; i++)
            {
                Assert.That(actualBranches[i], Is.EqualTo(expectedBranches[i]));
            }
        }
        [Test, Order(5)]
        public async Task GetBranchTest()
        {
            // Arrange
            var mockBranchesRepository = new Mock<IRepository<string, Branches>>();
            var mockLogger = new Mock<ILogger<BranchesService>>();

            IBranchesAdminService branchesService = new BranchesService(mockBranchesRepository.Object, mockLogger.Object);


            string branchKeyToRetrieve = "123456";


            var expectedBranch = new Branches("BranchName", "Location", 1);

            mockBranchesRepository.Setup(repo => repo.Get(branchKeyToRetrieve)).ReturnsAsync(expectedBranch);

            // Act
            var actualBranch = await branchesService.GetBranch(branchKeyToRetrieve);

            // Assert
            Assert.That(actualBranch, Is.EqualTo(expectedBranch));
        }


       








    }
}

