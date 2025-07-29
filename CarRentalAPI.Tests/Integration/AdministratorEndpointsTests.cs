using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CarRentalAPI.Domain.DTOs.Administrator.Request;
using CarRentalAPI.Domain.DTOs.Administrator.Response;
using CarRentalAPI.Domain.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CarRentalAPI.Tests.Integration;

[TestClass]
public class AdministratorEndpointsTests
{
    [TestMethod]
    public async Task Can_CRUD_Administrator()
    {
        var factory = new CustomWebApplicationFactory();
        var client = factory.CreateClient();

        // CREATE
        var createDto = new CreateAdministratorDto("admin2@email.com", "123456", AdminRoleEnum.Editor);
        var createResponse = await client.PostAsJsonAsync("/admin", createDto);
        Assert.AreEqual(HttpStatusCode.Created, createResponse.StatusCode);
        var created = await createResponse.Content.ReadFromJsonAsync<GetAdministratorDto>();
        Assert.IsNotNull(created);
        int id = created.Id;
        Assert.AreEqual(createDto.Email, created.Email);
        Assert.AreEqual(createDto.Role.ToString(), created.Role);

        // GET BY ID
        var getResponse = await client.GetAsync($"/admin/id/{id}");
        Assert.AreEqual(HttpStatusCode.OK, getResponse.StatusCode);
        var getAdmin = await getResponse.Content.ReadFromJsonAsync<GetAdministratorDto>();
        Assert.IsNotNull(getAdmin);
        Assert.AreEqual(createDto.Email, getAdmin.Email);

        // UPDATE
        var updateDto = new UpdateAdministratorDto(id, "updated@email.com", AdminRoleEnum.Admin);
        var updateResponse = await client.PutAsJsonAsync($"/admin/{id}", updateDto);
        Assert.AreEqual(HttpStatusCode.OK, updateResponse.StatusCode);
        var updated = await updateResponse.Content.ReadFromJsonAsync<GetAdministratorDto>();
        Assert.IsNotNull(updated);
        Assert.AreEqual("updated@email.com", updated.Email);
        Assert.AreEqual(AdminRoleEnum.Admin.ToString(), updated.Role);

        // DELETE
        var deleteResponse = await client.DeleteAsync($"/admin/{id}");
        Assert.AreEqual(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        // GET BY ID (after delete)
        var getAfterDelete = await client.GetAsync($"/admin/id/{id}");
        Assert.AreEqual(HttpStatusCode.NotFound, getAfterDelete.StatusCode);
    }
}
