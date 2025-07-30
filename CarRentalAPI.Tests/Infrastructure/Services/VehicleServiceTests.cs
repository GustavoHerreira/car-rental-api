using CarRentalAPI.Domain.Entities;
using CarRentalAPI.Infrastructure.Database;
using CarRentalAPI.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using CarRentalAPI.Domain.Exceptions.Vehicles;

namespace CarRentalAPI.Tests.Infrastructure.Services;

[TestClass]
public class VehicleServiceTests
{
    private AppDbContext GetDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        return new AppDbContext(options);
    }

    [TestMethod]
    public async Task AddAsync_AddsVehicleToDb()
    {
        // Arrange
        var db = GetDbContext(nameof(AddAsync_AddsVehicleToDb));
        var service = new VehicleService(db);
        var vehicle = new Vehicle { Name = "Car", Brand = "Brand", Year = 2020 };

        // Act
        var result = await service.AddAsync(vehicle);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Car", result.Name);
        Assert.AreEqual(1, await db.Vehicles.CountAsync());
    }

    [TestMethod]
    public async Task GetByIdAsync_ReturnsCorrectVehicle()
    {
        // Arrange
        var db = GetDbContext(nameof(GetByIdAsync_ReturnsCorrectVehicle));
        db.Vehicles.Add(new Vehicle { Name = "Test", Brand = "B", Year = 2021 });
        db.SaveChanges();
        var service = new VehicleService(db);

        // Act
        var vehicle = await service.GetByIdAsync(1);

        // Assert
        Assert.IsNotNull(vehicle);
        Assert.AreEqual("Test", vehicle.Name);
    }

    [TestMethod]
    public async Task GetAllAsync_ReturnsAllVehicles()
    {
        // Arrange
        var db = GetDbContext(nameof(GetAllAsync_ReturnsAllVehicles));
        db.Vehicles.Add(new Vehicle { Name = "Car1", Brand = "BrandA", Year = 2020 });
        db.Vehicles.Add(new Vehicle { Name = "Car2", Brand = "BrandB", Year = 2021 });
        db.SaveChanges();
        var service = new VehicleService(db);

        // Act
        var vehicles = await service.GetAllAsync();

        // Assert
        Assert.AreEqual(2, vehicles.Count);
    }

    [TestMethod]
    public async Task DeleteAsync_RemovesVehicle()
    {
        // Arrange
        var db = GetDbContext(nameof(DeleteAsync_RemovesVehicle));
        db.Vehicles.Add(new Vehicle { Name = "Del", Brand = "B", Year = 2022 });
        db.SaveChanges();
        var service = new VehicleService(db);

        // Act
        var result = await service.DeleteAsync(1);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(0, await db.Vehicles.CountAsync());
    }

    [TestMethod]
    public async Task UpdateAsync_UpdatesVehicleFields()
    {
        // Arrange
        var db = GetDbContext(nameof(UpdateAsync_UpdatesVehicleFields));
        var vehicle = new Vehicle { Name = "OldName", Brand = "OldBrand", Year = 2019 };
        db.Vehicles.Add(vehicle);
        db.SaveChanges();
        var service = new VehicleService(db);
        vehicle.Name = "NewName";
        vehicle.Brand = "NewBrand";
        vehicle.Year = 2022;

        // Act
        var result = await service.UpdateAsync(vehicle.Id, vehicle);

        // Assert
        Assert.AreEqual("NewName", result.Name);
        Assert.AreEqual("NewBrand", result.Brand);
        Assert.AreEqual(2022, result.Year);
    }

    [TestMethod]
    public async Task UpdateAsync_ThrowsVehicleNotFound_WhenIdDoesNotExist()
    {
        // Arrange
        var db = GetDbContext(nameof(UpdateAsync_ThrowsVehicleNotFound_WhenIdDoesNotExist));
        var service = new VehicleService(db);
        var updatedVehicle = new Vehicle { Id = 999, Name = "DoesNotExist", Brand = "Brand", Year = 2022 };

        // Act & Assert
        await Assert.ThrowsExceptionAsync<VehicleNotFound>(async () =>
        {
            await service.UpdateAsync(999, updatedVehicle);
        });
    }

    [TestMethod]
    public async Task UpdateAsync_ThrowsArgumentException_WhenIdsDoNotMatch()
    {
        // Arrange
        var db = GetDbContext(nameof(UpdateAsync_ThrowsArgumentException_WhenIdsDoNotMatch));
        db.Vehicles.Add(new Vehicle { Id = 1, Name = "Car", Brand = "Brand", Year = 2020 });
        db.SaveChanges();
        var service = new VehicleService(db);
        var updatedVehicle = new Vehicle { Id = 2, Name = "Car", Brand = "Brand", Year = 2020 };

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
        {
            await service.UpdateAsync(1, updatedVehicle);
        });
    }
}
