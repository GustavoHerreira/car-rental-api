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
        var created = await createResponse.Content.ReadFromJsonAsync<dynamic>();
        int id = created.id;
        Assert.AreEqual(vehicle.Name, (string)created.name);
        Assert.AreEqual(vehicle.Brand, (string)created.brand);
        Assert.AreEqual(vehicle.Year, (int)created.year);

        // GET BY ID
        var getResponse = await client.GetAsync($"/vehicle/{id}");
        Assert.AreEqual(HttpStatusCode.OK, getResponse.StatusCode);
        var getVehicle = await getResponse.Content.ReadFromJsonAsync<dynamic>();
        Assert.AreEqual(vehicle.Name, (string)getVehicle.name);

        // UPDATE
        vehicle.Name = "UpdatedCar";
        var updateResponse = await client.PutAsJsonAsync($"/vehicle/{id}", vehicle);
        Assert.AreEqual(HttpStatusCode.OK, updateResponse.StatusCode);
        var updated = await updateResponse.Content.ReadFromJsonAsync<dynamic>();
        Assert.AreEqual("UpdatedCar", (string)updated.name);

        // DELETE
        var deleteResponse = await client.DeleteAsync($"/vehicle/{id}");
        Assert.AreEqual(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        // GET BY ID (after delete)
        var getAfterDelete = await client.GetAsync($"/vehicle/{id}");
        Assert.AreEqual(HttpStatusCode.NotFound, getAfterDelete.StatusCode);
    }
}
