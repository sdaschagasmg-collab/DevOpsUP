# ---------- STAGE 1: Build ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 1) Copiamos SOLO los archivos de proyecto primero (csproj)
COPY DevOpsUP.slnx ./
COPY src/ProductsApi/ProductsApi.csproj src/ProductsApi/
COPY tests/ProductsApi.Tests/ProductsApi.Tests.csproj tests/ProductsApi.Tests/

# 2) Restauramos dependencias (capa cacheable)
RUN dotnet restore src/ProductsApi/ProductsApi.csproj

# 3) Copiamos el resto del código fuente
COPY src/ ./src/
COPY tests/ ./tests/

# 4) Publicamos la app en modo Release
RUN dotnet publish src/ProductsApi/ProductsApi.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# ---------- STAGE 2: Runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0-noble-chiseled AS final
WORKDIR /app

# Usuario non-root (ya viene por defecto en esta imagen, se deja explícito)
USER app

COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "ProductsApi.dll"]