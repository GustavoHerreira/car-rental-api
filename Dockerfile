# 1. Estágio de Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# ---- CORREÇÃO AQUI ----
# Copia o arquivo .csproj de dentro da pasta com nome diferente.
# Pasta de origem: "CarRentalAPI.API/CarRentalAPI.csproj"
# Pasta de destino no container: "CarRentalAPI.API/"
COPY ["CarRentalAPI.API/CarRentalAPI.csproj", "CarRentalAPI.API/"]

# Assumindo que o projeto de teste segue um padrão semelhante
# Se não tiver testes, pode remover esta linha.
COPY ["CarRentalAPI.Tests/CarRentalAPI.Tests.csproj", "CarRentalAPI.Tests/"]

# Copia o arquivo de solução
COPY ["CarRentalAPI.sln", "./"]

# Restaura as dependências para todos os projetos na solução
RUN dotnet restore "CarRentalAPI.sln"

# Copia todo o resto do código fonte
COPY . .

# Publica a aplicação
RUN dotnet publish "CarRentalAPI.API/CarRentalAPI.csproj" -c Release -o /app/publish --no-restore

# 2. Estágio Final
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

# ---- CORREÇÃO FINAL ----
# O nome da DLL é baseado no nome do arquivo .csproj, que é "CarRentalAPI.csproj".
# Portanto, a DLL será "CarRentalAPI.dll".
ENTRYPOINT ["dotnet", "CarRentalAPI.dll"]