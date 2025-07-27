using CarRentalAPI.Domain.Interfaces;
using CarRentalAPI.Domain.ModelViews;
using CarRentalAPI.Endpoints;
using CarRentalAPI.Infrastructure.Database;
using CarRentalAPI.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DI
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IAdministratorService, AdministratorService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();

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
    
    // Uses NSwag.AspNetCore UI
    app.UseSwaggerUi(options =>
        options.DocumentPath = "/openapi/v1.json");
}

app.UseHttpsRedirection();

// Redireciona a root pro SwaggerUI
app.MapGet("/", () => Results.Redirect("/swagger", permanent: true))
    .ExcludeFromDescription(); // Isso oculta o endpoint root do Swagger UI

// Chamada dos métodos de extensão para mapear os endpoints
app.MapAuthenticationEndpoints();
app.MapVehicleEndpoints();
app.MapAdministratorEndpoints();

app.Run();