using CarRentalAPI.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CarRentalAPI.Tests.Domain;

[TestClass]
public class VehicleTests
{
    [TestMethod]
    public void Vehicle_CanBeCreated_WithValidProperties()
    {
        var vehicle = new Vehicle
        {
            Id = 10,
            Name = "Model S",
            Brand = "Tesla",
            Year = 2022
        };

        Assert.AreEqual(10, vehicle.Id);
        Assert.AreEqual("Model S", vehicle.Name);
        Assert.AreEqual("Tesla", vehicle.Brand);
        Assert.AreEqual(2022, vehicle.Year);
    }

    [TestMethod]
    public void Vehicle_DefaultValues_AreCorrect()
    {
        var vehicle = new Vehicle();
        Assert.AreEqual(0, vehicle.Id);
        Assert.AreEqual(string.Empty, vehicle.Name);
        Assert.AreEqual(string.Empty, vehicle.Brand);
        Assert.AreEqual(0, vehicle.Year);
    }
}
