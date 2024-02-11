using System;
using Capstone_Project.Models.DTOs;
using Capstone_Project.Services;
using Moq;
using NUnit.Framework; // Add this using directive
using Microsoft.Extensions.Configuration; // Add this using directive
using System.IdentityModel.Tokens.Jwt;

namespace Capstone_ProjectTest
{
    public class TokenServiceTest
    {
        private Mock<IConfiguration> _mockConfiguration;
        private TokenService _tokenService;

        [SetUp]
        public void Setup()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.SetupGet(x => x["SecretKey"]).Returns("This is the dummy key that I use for this token");

            _tokenService = new TokenService(_mockConfiguration.Object);
        }

        [Test]
        public async Task GenerateToken_ValidInput_ReturnsToken()
        {
            // Arrange
            var user = new LoginUserDTO
            {
                Email = "test@example.com",
                UserType = "Admin"
            };

            // Act
            var token = await _tokenService.GenerateToken(user);

            // Assert
            Assert.IsNotNull(token);
            Assert.IsNotEmpty(token);
        }

        [Test]
        public async Task GenerateToken_ExpiredToken_ReturnsExpiredToken()
        {
            // Arrange
            var user = new LoginUserDTO
            {
                Email = "test@example.com",
                UserType = "Admin"
            };

            // Act
            var token = await _tokenService.GenerateToken(user);

            // Decode token
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            // Assert
            Assert.IsNotNull(jwtToken);
            Assert.IsTrue(jwtToken.ValidTo > DateTime.UtcNow);
        }
    }
}
