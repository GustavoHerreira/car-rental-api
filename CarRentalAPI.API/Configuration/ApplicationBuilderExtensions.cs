using CarRentalAPI.Infrastructure.Database;
using CarRentalAPI.Presentation.API;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Configuration;

public static class ApplicationBuilderExtensions
{
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        
        app.UseOpenApi();
        app.UseSwaggerUi();
        
        app.UseCors("cors_policy");
        
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<AppDbContext>(); 
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Ocorreu um erro durante a aplicação das migrations.");
            }
        }
        
        // Deploy na AWS via HTTP apenas
        // app.UseHttpsRedirection();

        app.MapGet("/", () => Results.Redirect("/swagger", permanent: true))
            .ExcludeFromDescription(); // remove do swagger/nswag
        
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }

    public static WebApplication ConfigureEndpoints(this WebApplication app)
    {
        app.MapAuthenticationEndpoints();
        app.MapVehicleEndpoints();
        app.MapAdministratorEndpoints();

        return app;
    }
}