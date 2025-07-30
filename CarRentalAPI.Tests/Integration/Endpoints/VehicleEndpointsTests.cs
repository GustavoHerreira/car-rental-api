using CarRentalAPI.Domain.Entities;
using CarRentalAPI.Tests.Integration.Config;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http.Json;

namespace CarRentalAPI.Tests.Integration.Endpoints;

[TestClass]
public class VehicleEndpointsTests
{
    private static CustomWebApplicationFactory _factory = null!;

    [ClassInitialize]
    public static void ClassInitialize(TestContext _)
    {
        _factory = new CustomWebApplicationFactory();
    }

    [TestMethod]
    public async Task CreateVehicle_ShouldReturnCreated()
    {
        // Arrange
        var client = await _factory.CreateClientWithAdminTokenAsync();
        var newVehicle = new Vehicle
        {
            Brand = "Toyota",
            Name = "Corolla",
            Year = 2022
        };

        // Act
        var response = await client.PostAsJsonAsync("/vehicle/", newVehicle);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdVehicle = await response.Content.ReadFromJsonAsync<Vehicle>();
        createdVehicle.Should().NotBeNull();
        createdVehicle.Brand.Should().Be(newVehicle.Brand);
        createdVehicle.Name.Should().Be(newVehicle.Name);
        createdVehicle.Year.Should().Be(newVehicle.Year);
    }

    [TestMethod]
    public async Task CreateVehicle_ShouldReturnBadRequest_WhenValidationFails()
    {
        // Arrange
        var client = await _factory.CreateClientWithAdminTokenAsync();
        var invalidVehicle = new Vehicle
        {
            Brand = "",
            Name = "",
            Year = 1940
        };
        // Act
        var response = await client.PostAsJsonAsync("/vehicle/", invalidVehicle);
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await response.Content.ReadAsStringAsync();
        error.Should().Contain("Nome do Modelo");
        error.Should().Contain("Nome da Fabricante");
        error.Should().Contain("Ano deve ser superior a 1950");
    }

    [TestMethod]
    public async Task UpdateVehicle_ShouldReturnBadRequest_WhenValidationFails()
    {
        // Arrange
        var client = await _factory.CreateClientWithAdminTokenAsync();
        var newVehicle = new Vehicle { Brand = "Ford", Name = "Ka", Year = 2018 };
        var createResponse = await client.PostAsJsonAsync("/vehicle/", newVehicle);
        var created = await createResponse.Content.ReadFromJsonAsync<Vehicle>();
        var invalidUpdate = new Vehicle { Id = created.Id, Brand = "", Name = "", Year = 2050 };
        // Act
        var response = await client.PutAsJsonAsync($"/vehicle/{created.Id}", invalidUpdate);
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await response.Content.ReadAsStringAsync();
        error.Should().Contain("Nome do Modelo");
        error.Should().Contain("Nome da Fabricante");
        error.Should().Contain("O ano deve ser menor que o ano atual");
    }

    [TestMethod]
    public async Task GetAllVehicles_ShouldReturnOk()
    {
        // Arrange
        var client = await _factory.CreateClientWithAdminTokenAsync();
        // Act
        var response = await client.GetAsync("/vehicle/");
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [TestMethod]
    public async Task GetVehicleById_ShouldReturnOkOrNotFound()
    {
        // Arrange
        var client = await _factory.CreateClientWithAdminTokenAsync();
        var newVehicle = new Vehicle { Brand = "VW", Name = "Gol", Year = 2020 };
        var createResponse = await client.PostAsJsonAsync("/vehicle/", newVehicle);
        var created = await createResponse.Content.ReadFromJsonAsync<Vehicle>();
        // Act
        var response = await client.GetAsync($"/vehicle/{created.Id}");
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        // Act
        var notFound = await client.GetAsync("/vehicle/99999");
        // Assert
        notFound.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task UpdateVehicle_ShouldReturnOk()
    {
        // Arrange
        var client = await _factory.CreateClientWithAdminTokenAsync();
        var newVehicle = new Vehicle { Brand = "Ford", Name = "Ka", Year = 2018 };
        var createResponse = await client.PostAsJsonAsync("/vehicle/", newVehicle);
        var created = await createResponse.Content.ReadFromJsonAsync<Vehicle>();
        var updated = new Vehicle { Id = created.Id, Brand = "Ford", Name = "Ka+", Year = 2019 };
        // Act
        var response = await client.PutAsJsonAsync($"/vehicle/{created.Id}", updated);
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        // Verify that the vehicle was actually updated and not a new one created
        var getResponse = await client.GetAsync($"/vehicle/{created.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var vehicleFromApi = await getResponse.Content.ReadFromJsonAsync<Vehicle>();
        vehicleFromApi.Should().NotBeNull();
        vehicleFromApi.Id.Should().Be(created.Id);
        vehicleFromApi.Brand.Should().Be(updated.Brand);
        vehicleFromApi.Name.Should().Be(updated.Name);
        vehicleFromApi.Year.Should().Be(updated.Year);
    }

    [TestMethod]
    public async Task DeleteVehicle_ShouldReturnNoContentOrNotFound()
    {
        // Arrange
        var client = await _factory.CreateClientWithAdminTokenAsync();
        var newVehicle = new Vehicle { Brand = "Fiat", Name = "Uno", Year = 2015 };
        var createResponse = await client.PostAsJsonAsync("/vehicle/", newVehicle);
        var created = await createResponse.Content.ReadFromJsonAsync<Vehicle>();
        // Act
        var response = await client.DeleteAsync($"/vehicle/{created.Id}");
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        // Act
        var notFound = await client.DeleteAsync("/vehicle/99999");
        // Assert
        notFound.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        _factory.Dispose();
    }
}