using CarRentalAPI.Domain.Entities;
using CarRentalAPI.Domain.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CarRentalAPI.Tests.Domain;

[TestClass]
public class AdministratorTests
{
    [TestMethod]
    public void Administrator_CanBeCreated_WithValidProperties()
    {
        var admin = new Administrator
        {
            Id = 1,
            Email = "admin@email.com",
            Password = "securepassword",
            Role = AdminRoleEnum.Admin
        };

        Assert.AreEqual(1, admin.Id);
        Assert.AreEqual("admin@email.com", admin.Email);
        Assert.AreEqual("securepassword", admin.Password);
        Assert.AreEqual(AdminRoleEnum.Admin, admin.Role);
    }

    [TestMethod]
    public void Administrator_DefaultValues_AreCorrect()
    {
        var admin = new Administrator();
        Assert.AreEqual(0, admin.Id);
        Assert.AreEqual(string.Empty, admin.Email);
        Assert.AreEqual(string.Empty, admin.Password);
        Assert.AreEqual(AdminRoleEnum.Admin, admin.Role);
    }
}
