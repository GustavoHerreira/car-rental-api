using CarRentalAPI.Domain.Entities;
using CarRentalAPI.Infrastructure.Database;
using CarRentalAPI.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace CarRentalAPI.Tests.Infrastructure.Services;

[TestClass]
public class VehicleServiceTests
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
    public async Task AddAsync_AddsVehicleToDb()
    {
        var db = GetDbContext(nameof(AddAsync_AddsVehicleToDb));
        var service = new VehicleService(db);
        var vehicle = new Vehicle { Name = "Car", Brand = "Brand", Year = 2020 };
        var result = await service.AddAsync(vehicle);
        Assert.IsNotNull(result);
        Assert.AreEqual("Car", result.Name);
        Assert.AreEqual(1, await db.Vehicles.CountAsync());
    }

    [TestMethod]
    public async Task GetByIdAsync_ReturnsCorrectVehicle()
    {
        var db = GetDbContext(nameof(GetByIdAsync_ReturnsCorrectVehicle));
        db.Vehicles.Add(new Vehicle { Name = "Test", Brand = "B", Year = 2021 });
        db.SaveChanges();
        var service = new VehicleService(db);
        var vehicle = await service.GetByIdAsync(1);
        Assert.IsNotNull(vehicle);
        Assert.AreEqual("Test", vehicle.Name);
    }

    [TestMethod]
    public async Task DeleteAsync_RemovesVehicle()
    {
        var db = GetDbContext(nameof(DeleteAsync_RemovesVehicle));
        db.Vehicles.Add(new Vehicle { Name = "Del", Brand = "B", Year = 2022 });
        db.SaveChanges();
        var service = new VehicleService(db);
        var result = await service.DeleteAsync(1);
        Assert.IsTrue(result);
        Assert.AreEqual(0, await db.Vehicles.CountAsync());
    }
}
