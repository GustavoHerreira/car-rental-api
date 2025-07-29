using CarRentalAPI.Domain.DTOs.Administrator.Request;
using CarRentalAPI.Domain.DTOs.Administrator.Response;
using CarRentalAPI.Domain.Entities;
using CarRentalAPI.Domain.Enums;
using CarRentalAPI.Infrastructure.Database;
using CarRentalAPI.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace CarRentalAPI.Tests.Infrastructure.Services;

[TestClass]
public class AdministratorServiceTests
{
    private AppDbContext GetDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        var configuration = new ConfigurationBuilder().AddInMemoryCollection().Build();
        return new AppDbContext(options, configuration);
    }

    [TestMethod]
    public async Task CreateAdministrator_AddsAdministratorToDb()
    {
        var db = GetDbContext(nameof(CreateAdministrator_AddsAdministratorToDb));
        var service = new AdministratorService(db);
        var dto = new CreateAdministratorDto("test@email.com", "pass", AdminRoleEnum.Admin);
        var result = await service.CreateAdministrator(dto);
        Assert.IsNotNull(result);
        Assert.AreEqual(dto.Email, result.Email);
        Assert.AreEqual(dto.Role.ToString(), result.Role);
    }

    [TestMethod]
    public async Task GetAdministratorById_ReturnsCorrectAdmin()
    {
        var db = GetDbContext(nameof(GetAdministratorById_ReturnsCorrectAdmin));
        db.Administrators.Add(new Administrator { Email = "a@b.com", Password = "p", Role = AdminRoleEnum.Admin });
        db.SaveChanges();
        var service = new AdministratorService(db);
        var admin = await service.GetAdministratorById(1);
        Assert.IsNotNull(admin);
        Assert.AreEqual("a@b.com", admin.Email);
    }

    [TestMethod]
    public async Task DeleteAdministrator_RemovesAdmin()
    {
        var db = GetDbContext(nameof(DeleteAdministrator_RemovesAdmin));
        db.Administrators.Add(new Administrator { Email = "del@a.com", Password = "p", Role = AdminRoleEnum.Admin });
        db.SaveChanges();
        var service = new AdministratorService(db);
        var result = await service.DeleteAdministrator(1);
        Assert.IsTrue(result);
        Assert.AreEqual(0, await db.Administrators.CountAsync());
    }
}
