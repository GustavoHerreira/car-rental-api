using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using CarRentalAPI.Infrastructure.Database;
using Microsoft.AspNetCore.TestHost;
using CarRentalAPI.Domain.Entities;
using CarRentalAPI.Domain.Enums;
using CarRentalAPI.Domain.DTOs.Authentication;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Identity;
using CarRentalAPI.Domain.ModelViews;
using CarRentalAPI.Configuration;

namespace CarRentalAPI.Tests.Integration.Config;

class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _adminEmail = $"admin-{Guid.NewGuid()}@test.com";
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Define o ambiente para evitar conflito com a aplicação principal (API)
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Testing.json"));
        });

        builder.ConfigureTestServices(services =>
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Testing.json")
                .Build();

            services.ConfigureAuthentication(configuration);

            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(DatabaseFixture.ConnectionString);
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();

            // Seed the database with a test admin user
            var adminUser = new Administrator
            {
                Email = _adminEmail,
                Password = "password123",
                Role = AdminRoleEnum.Admin
            };
            db.Administrators.Add(adminUser);
            db.SaveChanges();
        });
    }
    
    public async Task<HttpClient> CreateClientWithAdminTokenAsync()
    {
        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/auth/admin/login", new LoginDto(_adminEmail, "password123"));
        response.EnsureSuccessStatusCode();
        var loggedAdmin = await response.Content.ReadFromJsonAsync<LoggedAdmin>();
        var token = loggedAdmin?.Token;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }
}