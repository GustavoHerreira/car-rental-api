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
        
        return new AppDbContext(options);
    }

    [TestMethod]
    public async Task CreateAdministrator_AddsAdministratorToDb()
    {
        // Arrange
        var db = GetDbContext(nameof(CreateAdministrator_AddsAdministratorToDb));
        var service = new AdministratorService(db);
        var dto = new CreateAdministratorDto("test@email.com", "pass", AdminRoleEnum.Admin);

        // Act
        var result = await service.CreateAdministrator(dto);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(dto.Email, result.Email);
        Assert.AreEqual(dto.Role.ToString(), result.Role);
    }

    [TestMethod]
    public async Task GetAdministratorByEmail_ReturnsCorrectAdmin()
    {
        // Arrange
        var db = GetDbContext(nameof(GetAdministratorByEmail_ReturnsCorrectAdmin));
        db.Administrators.Add(new Administrator { Email = "find@me.com", Password = "pw", Role = AdminRoleEnum.Admin });
        db.SaveChanges();
        var service = new AdministratorService(db);

        // Act
        var admin = await service.GetAdministratorByEmail("find@me.com");

        // Assert
        Assert.IsNotNull(admin);
        Assert.AreEqual("find@me.com", admin.Email);
    }

    [TestMethod]
    public async Task GetAdministratorByLoginAndPassword_ReturnsCorrectAdmin()
    {
        // Arrange
        var db = GetDbContext(nameof(GetAdministratorByLoginAndPassword_ReturnsCorrectAdmin));
        db.Administrators.Add(new Administrator { Email = "login@me.com", Password = "pw123", Role = AdminRoleEnum.Admin });
        db.SaveChanges();
        var service = new AdministratorService(db);
        var loginDto = new CarRentalAPI.Domain.DTOs.Authentication.LoginDto("login@me.com", "pw123");

        // Act
        var admin = await service.GetAdministratorByLoginAndPassword(loginDto);

        // Assert
        Assert.IsNotNull(admin);
        Assert.AreEqual("login@me.com", admin.Email);
    }

    [TestMethod]
    public async Task GetAdministratorById_ReturnsCorrectAdmin()
    {
        // Arrange
        var db = GetDbContext(nameof(GetAdministratorById_ReturnsCorrectAdmin));
        db.Administrators.Add(new Administrator { Email = "a@b.com", Password = "p", Role = AdminRoleEnum.Admin });
        db.SaveChanges();
        var service = new AdministratorService(db);

        // Act
        var admin = await service.GetAdministratorById(1);

        // Assert
        Assert.IsNotNull(admin);
        Assert.AreEqual("a@b.com", admin.Email);
    }

    [TestMethod]
    public async Task GetAllAdministrators_ReturnsAllAdmins()
    {
        // Arrange
        var db = GetDbContext(nameof(GetAllAdministrators_ReturnsAllAdmins));
        db.Administrators.Add(new Administrator { Email = "a1@b.com", Password = "p1", Role = AdminRoleEnum.Admin });
        db.Administrators.Add(new Administrator { Email = "a2@b.com", Password = "p2", Role = AdminRoleEnum.Admin });
        db.SaveChanges();
        var service = new AdministratorService(db);

        // Act
        var admins = await service.GetAllAdministrators();

        // Assert
        Assert.AreEqual(2, admins.Count);
    }

    [TestMethod]
    public async Task DeleteAdministrator_RemovesAdmin()
    {
        // Arrange
        var db = GetDbContext(nameof(DeleteAdministrator_RemovesAdmin));
        db.Administrators.Add(new Administrator { Email = "del@a.com", Password = "p", Role = AdminRoleEnum.Admin });
        db.SaveChanges();
        var service = new AdministratorService(db);

        // Act
        var result = await service.DeleteAdministrator(1);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(0, await db.Administrators.CountAsync());
    }

    [TestMethod]
    public async Task UpdateAdministrator_UpdatesAdminFields()
    {
        // Arrange
        var db = GetDbContext(nameof(UpdateAdministrator_UpdatesAdminFields));
        db.Administrators.Add(new Administrator { Id = 1, Email = "old@email.com", Password = "pw", Role = AdminRoleEnum.Admin });
        db.SaveChanges();
        var service = new AdministratorService(db);
        var updateDto = new UpdateAdministratorDto(1, "new@email.com", AdminRoleEnum.Editor);

        // Act
        var updated = await service.UpdateAdministrator(1, updateDto);

        // Assert
        Assert.AreEqual("new@email.com", updated.Email);
        Assert.AreEqual(AdminRoleEnum.Editor.ToString(), updated.Role);
    }
}
