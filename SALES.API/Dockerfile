# Etapa 1: Compilación y Publicación
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar archivos de proyecto (.csproj) para restaurar dependencias
# Se han adaptado los nombres según tu solución SALES
COPY ["SALES.API/SALES.API.csproj", "SALES.API/"]
COPY ["SALES.Application/SALES.Application.csproj", "SALES.Application/"]
COPY ["SALES.Domain/SALES.Domain.csproj", "SALES.Domain/"]
COPY ["SALES.Infrastructure/SALES.Infrastructure.csproj", "SALES.Infrastructure/"]
COPY ["SALES.Persistence/SALES.Persistence.csproj", "SALES.Persistence/"]
COPY ["SALES.Tests.Unit/SALES.Tests.Unit.csproj", "SALES.Tests.Unit/"]

# Restaurar paquetes NuGet de la API principal
RUN dotnet restore "SALES.API/SALES.API.csproj"

# Copiar todo el código fuente de la solución
COPY . .

# Compilar el proyecto de la API en modo Release
WORKDIR "/src/SALES.API"
RUN dotnet build "SALES.API.csproj" -c Release -o /app/build

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish "SALES.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: Imagen de ejecución (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .

# Comando de inicio usando la DLL de Sales
ENTRYPOINT ["dotnet", "SALES.API.dll"]