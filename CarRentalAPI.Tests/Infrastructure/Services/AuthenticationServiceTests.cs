using System.Threading.Tasks;
using CarRentalAPI.Domain.DTOs.Authentication;
using CarRentalAPI.Domain.Entities;
using CarRentalAPI.Domain.Enums;
using CarRentalAPI.Domain.Interfaces;
using CarRentalAPI.Domain.ModelViews;
using CarRentalAPI.Infrastructure.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CarRentalAPI.Tests.Infrastructure.Services;

[TestClass]
public class AuthenticationServiceTests
{
    [TestMethod]
    public async Task Login_ReturnsLoggedAdmin_WhenCredentialsAreValid()
    {
        // Arrange
        var loginDto = new LoginDto("admin@email.com", "123456");
        var admin = new Administrator { Id = 1, Email = loginDto.Email, Password = loginDto.Password, Role = AdminRoleEnum.Admin };
        var mockAdminService = new Mock<IAdministratorService>();
        mockAdminService.Setup(s => s.GetAdministratorByLoginAndPassword(loginDto)).ReturnsAsync(admin);
        var jwtKey = "test_jwt_key_12345678901234567890123456789012"; // 32+ chars for HMAC
        var authService = new AuthenticationService(mockAdminService.Object, jwtKey);

        // Act
        var result = await authService.Login(loginDto);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(admin.Id, result.Id);
        Assert.AreEqual(admin.Email, result.Email);
        Assert.AreEqual(admin.Role.ToString(), result.Role);
        Assert.IsFalse(string.IsNullOrEmpty(result.Token));
    }

    [TestMethod]
    public async Task Login_ReturnsNull_WhenCredentialsAreInvalid()
    {
        // Arrange
        var loginDto = new LoginDto("wrong@email.com", "wrongpass");
        var mockAdminService = new Mock<IAdministratorService>();
        mockAdminService.Setup(s => s.GetAdministratorByLoginAndPassword(loginDto)).ReturnsAsync((Administrator?)null);
        var jwtKey = "test_jwt_key_12345678901234567890123456789012";
        var authService = new AuthenticationService(mockAdminService.Object, jwtKey);

        // Act
        var result = await authService.Login(loginDto);

        // Assert
        Assert.IsNull(result);
    }
}
