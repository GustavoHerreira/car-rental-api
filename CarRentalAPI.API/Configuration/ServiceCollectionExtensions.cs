using System.Security.Claims;
using System.Text;
using CarRentalAPI.Domain.Enums;
using CarRentalAPI.Application.Interfaces;
using CarRentalAPI.Infrastructure.Database;
using CarRentalAPI.Infrastructure.Services;
using CarRentalAPI.Configuration.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace CarRentalAPI.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuração fortemente tipada para JWT
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IAdministratorService, AdministratorService>();
        services.AddScoped<IVehicleService, VehicleService>();

        services.AddEndpointsApiExplorer();
        
        // Exemplo para produção
        services.AddCors(options =>
        {
            options.AddPolicy(name: "cors_policy",
                policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
        
        // ENUM as string
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        });
        
        return services;
    }

    public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection("Jwt").Get<JwtOptions>() ??
                         throw new InvalidOperationException("Configuração 'Jwt' não encontrada.");
        
        var jwtKey = jwtOptions.Key;
        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new InvalidOperationException("A chave JWT não está configurada na seção 'Jwt'.");
        }

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; // fallback
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                // Valida a assinatura
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),

                // Valida o tempo de expiração
                ValidateLifetime = true,
                ValidateIssuer = false,
                ValidateAudience = false,

                NameClaimType = ClaimTypes.NameIdentifier,
                RoleClaimType = ClaimTypes.Role
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Append("Token-Expired", "true");
                    }
                    return Task.CompletedTask;
                }
            };
        });

        // Configuração de autorização com Roles
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => 
                policy.RequireRole(nameof(AdminRoleEnum.Admin)));

            options.AddPolicy("EditorOrAdmin", policy =>
                policy.RequireRole(
                    nameof(AdminRoleEnum.Admin),
                    nameof(AdminRoleEnum.Editor)
                ));
        });
        
        return services;
    }

    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddOpenApiDocument(options =>
        {
            // Informações básicas
            options.DocumentName = "v1";
            options.Title = "Car Rental API";
            options.Version = "v1";
            options.Description = "API de Aluguel de Carros desenvolvida com ASP.NET Core Minimal APIs. " +
                                  "[Código fonte disponível no GitHub](https://github.com/GustavoHerreira/car-rental-api)";
            
            // Autenticação JWT no Swagger
            options.AddSecurity("JWT", new OpenApiSecurityScheme
            {
                // Bearer Token
                Type = OpenApiSecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Insira o token JWT:"
            });

            options.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT"));
        });
        
        return services;
    }

    public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        // Apenas registra o PostgreSQL se NÃO estiver em ambiente de testes
        if (!env.IsEnvironment("Testing"))
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection") ??
                    throw new InvalidOperationException("String de conexão 'DefaultConnection' não encontrada")));
        }
        return services;
    }
}