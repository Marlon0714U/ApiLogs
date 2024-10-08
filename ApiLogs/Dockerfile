# Usa la imagen base de .NET para la ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Usa la imagen SDK para la construcción y ejecución de comandos de migración
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ApiLogs.csproj", "./"]
RUN dotnet restore "ApiLogs.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "ApiLogs.csproj" -c Release -o /app/build

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish "ApiLogs.csproj" -c Release -o /app/publish

# Usar la imagen final para ejecutar la aplicación
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Copiar la base de datos que generaste localmente
COPY logs.db /app/logs.db

ENTRYPOINT ["dotnet", "ApiLogs.dll"]
