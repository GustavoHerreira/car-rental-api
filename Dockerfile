# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["CarRentalAPI.API/CarRentalAPI.csproj", "CarRentalAPI.API/"]
RUN dotnet restore "CarRentalAPI.API/CarRentalAPI.csproj"
COPY . .
RUN dotnet publish "CarRentalAPI.API/CarRentalAPI.csproj" -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CarRentalAPI.dll"]