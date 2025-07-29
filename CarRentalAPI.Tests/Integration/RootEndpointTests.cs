using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CarRentalAPI.Tests.Integration;

[TestClass]
public class RootEndpointTests
{
    [TestMethod]
    public async Task Get_Root_ShouldRedirectToSwagger()
    {
        // Arrange
        var factory = new CustomWebApplicationFactory();
        var client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        // Act
        var response = await client.GetAsync("/");

        // Assert
        Assert.IsTrue(
            response.StatusCode == HttpStatusCode.Redirect ||
            response.StatusCode == HttpStatusCode.MovedPermanently,
            $"Expected redirect, got: {response.StatusCode}");
        Assert.AreEqual("/swagger", response.Headers.Location?.ToString());
    }
}
