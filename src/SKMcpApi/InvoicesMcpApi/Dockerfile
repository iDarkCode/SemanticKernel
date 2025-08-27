# Etapa base runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar el csproj y restaurar
COPY ["src/SKMcpApi/InvoicesMcpApi/InvoicesMcpApi.csproj", "InvoicesMcpApi/"]
RUN dotnet restore "InvoicesMcpApi/InvoicesMcpApi.csproj"

# Copiar todo el proyecto
COPY ./src/SKMcpApi/InvoicesMcpApi/ InvoicesMcpApi/
WORKDIR "/src/InvoicesMcpApi"

# Build
RUN dotnet build "InvoicesMcpApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publicar
FROM build AS publish
RUN dotnet publish "InvoicesMcpApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InvoicesMcpApi.dll", "--urls", "http://0.0.0.0:8080"]
