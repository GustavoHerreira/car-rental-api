# Estágio de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copia arquivos de projeto e solução para restaurar pacotes em cache
COPY ["CarRentalAPI.API/CarRentalAPI.csproj", "CarRentalAPI.API/"]
COPY ["CarRentalAPI.Tests/CarRentalAPI.Tests.csproj", "CarRentalAPI.Tests/"]
COPY ["CarRentalAPI.sln", "./"]
RUN dotnet restore "CarRentalAPI.sln"

# Copia o resto do código e publica a aplicação
COPY . .
RUN dotnet publish "CarRentalAPI.API/CarRentalAPI.csproj" -c Release -o /app/publish --no-restore

# Estágio final (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CarRentalAPI.dll"]