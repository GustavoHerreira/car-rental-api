using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using CarRentalAPI.Infrastructure.Database;

namespace CarRentalAPI.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove all registrations for AppDbContext and DbContextOptions<AppDbContext>
            var descriptors = services.Where(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>) ||
                     d.ServiceType == typeof(AppDbContext)).ToList();
            foreach (var descriptor in descriptors)
                services.Remove(descriptor);

            // Add InMemory for testing
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });
        });
        return base.CreateHost(builder);
    }
}