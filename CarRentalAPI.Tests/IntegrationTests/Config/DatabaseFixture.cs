using CarRentalAPI.Infrastructure.Database;
using DotNet.Testcontainers.Builders;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace CarRentalAPI.Tests.Integration.Config;

[TestClass]
public static class DatabaseFixture
{
    private static readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:16")
        .WithDatabase("carrental_test")
        .WithUsername("user")
        .WithPassword("password")
        .WithPortBinding(5433, 5432)
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
        .Build();

    public static string ConnectionString => $"{_dbContainer.GetConnectionString()};Include Error Detail=true";

    [AssemblyInitialize]
    public static void Initialize(TestContext _)
    {
        _dbContainer.StartAsync().Wait();
        using var dbContext = CreateDbContext();
        dbContext.Database.Migrate();
    }

    [AssemblyCleanup]
    public static void Cleanup()
    {
        _dbContainer.StopAsync().Wait();
        _dbContainer.DisposeAsync().AsTask().Wait();
    }

    public static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;
        return new AppDbContext(options);
    }
}