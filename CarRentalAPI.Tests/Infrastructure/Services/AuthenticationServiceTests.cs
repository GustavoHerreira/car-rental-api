using CarRentalAPI.Domain.DTOs.Authentication;
using CarRentalAPI.Domain.Entities;
using CarRentalAPI.Domain.Enums;
using CarRentalAPI.Domain.Interfaces;
using CarRentalAPI.Infrastructure.Services;
using CarRentalAPI.Domain.Options;
using Microsoft.Extensions.Options;
using Moq;

namespace CarRentalAPI.Tests.Infrastructure.Services;

[TestClass]
public class AuthenticationServiceTests
{
    // Mocks e serviços declarados no escopo da classe
    private Mock<IAdministratorService> _mockAdminService = null!;
    private AuthenticationService _authService = null!;

    [TestInitialize]
    public void Setup()
    {
        _mockAdminService = new Mock<IAdministratorService>();

        // Key hardcoded porque é um teste de UNIDADE (isolado)
        const string jwtKey = "SecretKeyMinimalApiChaveSuperSeguraComTamanhoAdequado32";
        var jwtOptions = Options.Create(new JwtOptions { Key = jwtKey });

        _authService = new AuthenticationService(_mockAdminService.Object, jwtOptions);
    }

    [TestMethod]
    public async Task Login_ReturnsLoggedAdmin_WhenCredentialsAreValid()
    {
        // Arrange
        var loginDto = new LoginDto("admin@email.com", "123456");
        var admin = new Administrator { Id = 1, Email = loginDto.Email, Role = AdminRoleEnum.Admin };
        
        _mockAdminService.Setup(s => s.GetAdministratorByLoginAndPassword(loginDto))
                         .ReturnsAsync(admin);

        // Act
        var result = await _authService.Login(loginDto);

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

        _mockAdminService.Setup(s => s.GetAdministratorByLoginAndPassword(loginDto))
                         .ReturnsAsync((Administrator?)null);

        // Act
        var result = await _authService.Login(loginDto);

        // Assert
        Assert.IsNull(result);
    }
}