using CarRentalAPI.Presentation.API;

namespace CarRentalAPI.Configuration;

public static class ApplicationBuilderExtensions
{
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseOpenApi();
            app.UseSwaggerUi();
        }

        app.UseHttpsRedirection();

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