using CarRentalAPI.Domain.Interfaces;
using CarRentalAPI.Endpoints;
using CarRentalAPI.Infrastructure.Database;
using CarRentalAPI.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// DI
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IAdministratorService, AdministratorService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// JWT
var jwtKey = builder.Configuration.GetSection("Jwt").GetValue<string>("Key")
             ?? throw new Exception("The JWT token is not configured correctly");

// Isso permite que a jwtKey seja injetada em qualquer lugar da aplicação
builder.Services.AddSingleton(jwtKey);


builder.Services.AddAuthentication(configs =>
{
    configs.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    configs.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    configs.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtKey)),
    };
});
builder.Services.AddAuthorization();

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

app.UseAuthentication();
app.UseAuthorization();

app.Run();