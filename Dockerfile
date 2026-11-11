
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY . .

RUN dotnet restore \
    samples/01-api-design/src/Catalog.Api/Catalog.Api.csproj

RUN dotnet publish \
    samples/01-api-design/src/Catalog.Api/Catalog.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

USER $APP_UID

ENTRYPOINT ["dotnet", "Catalog.Api.dll"]
