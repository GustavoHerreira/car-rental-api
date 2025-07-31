using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using CarRentalAPI.Tests.Integration.Config;
using CarRentalAPI.Domain.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Claims;

namespace CarRentalAPI.Tests.Integration.ConfigTests;

[TestClass]
public class CustomWebApplicationFactoryTests
{
    private static CustomWebApplicationFactory _factory = null!;

    [ClassInitialize]
    public static void ClassInitialize(TestContext _)
    {
        _factory = new CustomWebApplicationFactory();
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        _factory.Dispose();
    }

    [TestMethod]
    public async Task CreateClientWithAdminTokenAsync_ShouldReturnClientWithAdminRoleClaim()
    {
        // Arrange
        var client = await _factory.CreateClientWithAdminTokenAsync();

        // Act
        var token = client.DefaultRequestHeaders.Authorization?.Parameter;
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
        var roleClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        // Assert
        Assert.IsNotNull(roleClaim, "Role claim should not be null.");
        Assert.AreEqual(nameof(AdminRoleEnum.Admin), roleClaim, "Role claim should be Administrator.");
    }
}