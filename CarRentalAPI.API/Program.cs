using CarRentalAPI.Configuration;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region Configuração de Serviços

        // Configuração dos serviços da aplicação
        builder.Services
            .ConfigureServices(builder.Configuration)
            .ConfigureAuthentication(builder.Configuration)
            .ConfigureSwagger()
            .ConfigureDatabase(builder.Configuration);

        #endregion

        var app = builder.Build();

        #region Configuração do Pipeline HTTP

        // Configuração do pipeline e endpoints da aplicação
        app.ConfigurePipeline()
           .ConfigureEndpoints();

        #endregion

        app.Run();
    }
}