using CarRentalAPI.Domain.DTOs.Authentication;
using CarRentalAPI.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Registra o AppDbContext com a string de conexão do appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello World!");

app.MapPost("/login", (LoginDto loginDto) => 
    loginDto is { Email: "admin@email.com", Password: "123456" } 
        ? Results.Ok("Login com sucesso")
        : Results.Unauthorized());

app.Run();