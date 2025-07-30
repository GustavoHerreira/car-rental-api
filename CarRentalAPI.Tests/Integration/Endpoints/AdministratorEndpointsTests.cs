using CarRentalAPI.Domain.DTOs.Administrator.Request;
using CarRentalAPI.Domain.DTOs.Administrator.Response;
using CarRentalAPI.Tests.Integration.Config;
using CarRentalAPI.Domain.Enums;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http.Json;

namespace CarRentalAPI.Tests.Integration.Endpoints;

[TestClass]
public class AdministratorEndpointsTests
{
    private static CustomWebApplicationFactory _factory = null!;

    [ClassInitialize]
    public static void ClassInitialize(TestContext _)
    {
        _factory = new CustomWebApplicationFactory();
    }

    [TestMethod]
    public async Task CreateAdministrator_ShouldReturnCreated()
    {
        // Arrange
        var client = await _factory.CreateClientWithAdminTokenAsync();
        var newAdmin = new CreateAdministratorDto(
            Email: "newadmin@test.com",
            Password: "password123",
            Role: AdminRoleEnum.Admin);

        // Act
        var response = await client.PostAsJsonAsync("/admin/", newAdmin);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdAdmin = await response.Content.ReadFromJsonAsync<GetAdministratorDto>();
        createdAdmin.Should().NotBeNull();
        createdAdmin.Email.Should().Be(newAdmin.Email);
        createdAdmin.Role.Should().Be(newAdmin.Role.ToString());
    }

    [TestMethod]
    public async Task GetAllAdministrators_ShouldReturnOk()
    {
        // Arrange
        var client = await _factory.CreateClientWithAdminTokenAsync();
        // Act
        var response = await client.GetAsync("/admin/");
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [TestMethod]
    public async Task GetAdministratorById_ShouldReturnOkOrNotFound()
    {
        // Arrange
        var client = await _factory.CreateClientWithAdminTokenAsync();
        var newAdmin = new CreateAdministratorDto("idtest@test.com", "pw", AdminRoleEnum.Admin);
        var createResponse = await client.PostAsJsonAsync("/admin/", newAdmin);
        var created = await createResponse.Content.ReadFromJsonAsync<GetAdministratorDto>();
        // Act
        var response = await client.GetAsync($"/admin/id/{created.Id}");
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        // Act
        var notFound = await client.GetAsync("/admin/id/99999");
        // Assert
        notFound.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task GetAdministratorByEmail_ShouldReturnOkOrNotFound()
    {
        // Arrange
        var client = await _factory.CreateClientWithAdminTokenAsync();
        var newAdmin = new CreateAdministratorDto("emailtest@test.com", "pw", AdminRoleEnum.Admin);
        await client.PostAsJsonAsync("/admin/", newAdmin);
        // Act
        var response = await client.GetAsync($"/admin/email/{newAdmin.Email}");
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        // Act
        var notFound = await client.GetAsync("/admin/email/notfound@test.com");
        // Assert
        notFound.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task UpdateAdministrator_ShouldReturnOk()
    {
        // Arrange
        var client = await _factory.CreateClientWithAdminTokenAsync();
        var newAdmin = new CreateAdministratorDto("updatetest@test.com", "pw", AdminRoleEnum.Admin);
        var createResponse = await client.PostAsJsonAsync("/admin/", newAdmin);
        var created = await createResponse.Content.ReadFromJsonAsync<GetAdministratorDto>();
        var updateDto = new UpdateAdministratorDto(created.Id, "updated@test.com", AdminRoleEnum.Editor);
        // Act
        var response = await client.PutAsJsonAsync($"/admin/{created.Id}", updateDto);
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [TestMethod]
    public async Task DeleteAdministrator_ShouldReturnNoContentOrNotFound()
    {
        // Arrange
        var client = await _factory.CreateClientWithAdminTokenAsync();
        var newAdmin = new CreateAdministratorDto("deletetest@test.com", "pw", AdminRoleEnum.Admin);
        var createResponse = await client.PostAsJsonAsync("/admin/", newAdmin);
        var created = await createResponse.Content.ReadFromJsonAsync<GetAdministratorDto>();
        // Act
        var response = await client.DeleteAsync($"/admin/{created?.Id}");
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        // Act
        var notFound = await client.DeleteAsync("/admin/99999");
        // Assert
        notFound.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task CreateAdministrator_WithDuplicateEmail_ShouldReturnError()
    {
        // Arrange
        var client = await _factory.CreateClientWithAdminTokenAsync();
        const string email = "duplicate@test.com";
        var admin1 = new CreateAdministratorDto(email, "pw1", AdminRoleEnum.Admin);
        var admin2 = new CreateAdministratorDto(email, "pw2", AdminRoleEnum.Admin);
        await client.PostAsJsonAsync("/admin/", admin1);
        // Act
        var response = await client.PostAsJsonAsync("/admin/", admin2);
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await response.Content.ReadAsStringAsync();
        error.Should().Contain("já está em uso");
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        _factory.Dispose();
    }
}