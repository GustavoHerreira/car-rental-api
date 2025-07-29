using CarRentalAPI.Domain.ModelViews;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CarRentalAPI.Tests.Domain;

[TestClass]
public class LoggedAdminTests
{
    [TestMethod]
    public void LoggedAdmin_CanBeCreated_WithValidProperties()
    {
        var loggedAdmin = new LoggedAdmin(1, "admin@email.com", "Admin", "token123");
        Assert.AreEqual(1, loggedAdmin.Id);
        Assert.AreEqual("admin@email.com", loggedAdmin.Email);
        Assert.AreEqual("Admin", loggedAdmin.Role);
        Assert.AreEqual("token123", loggedAdmin.Token);
    }
}
