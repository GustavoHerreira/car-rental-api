using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CarRentalAPI.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CarRentalAPI.Tests.Integration;

[TestClass]
public class VehicleEndpointsTests
{
    [TestMethod]
    public async Task Can_CRUD_Vehicle()
    {
        var factory = new CustomWebApplicationFactory();
        var client = factory.CreateClient();

        // CREATE
        var vehicle = new Vehicle { Name = "TestCar", Brand = "TestBrand", Year = 2024 };
        var createResponse = await client.PostAsJsonAsync("/vehicle", vehicle);
        Assert.AreEqual(HttpStatusCode.Created, createResponse.StatusCode);
        var created = await createResponse.Content.ReadFromJsonAsync<Vehicle>();
        Assert.IsNotNull(created);
        int id = created.Id;
        Assert.AreEqual(vehicle.Name, created.Name);
        Assert.AreEqual(vehicle.Brand, created.Brand);
        Assert.AreEqual(vehicle.Year, created.Year);

        // GET BY ID
        var getResponse = await client.GetAsync($"/vehicle/{id}");
        Assert.AreEqual(HttpStatusCode.OK, getResponse.StatusCode);
        var getVehicle = await getResponse.Content.ReadFromJsonAsync<Vehicle>();
        Assert.IsNotNull(getVehicle);
        Assert.AreEqual(vehicle.Name, getVehicle.Name);

        // UPDATE
        vehicle.Name = "UpdatedCar";
        var updateResponse = await client.PutAsJsonAsync($"/vehicle/{id}", vehicle);
        Assert.AreEqual(HttpStatusCode.OK, updateResponse.StatusCode);
        var updated = await updateResponse.Content.ReadFromJsonAsync<Vehicle>();
        Assert.IsNotNull(updated);
        Assert.AreEqual("UpdatedCar", updated.Name);

        // DELETE
        var deleteResponse = await client.DeleteAsync($"/vehicle/{id}");
        Assert.AreEqual(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        // GET BY ID (after delete)
        var getAfterDelete = await client.GetAsync($"/vehicle/{id}");
        Assert.AreEqual(HttpStatusCode.NotFound, getAfterDelete.StatusCode);
    }
}
