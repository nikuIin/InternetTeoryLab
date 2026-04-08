FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY WineShop.csproj .
RUN dotnet restore WineShop.csproj

# Copy all source files
COPY . .

# Build and publish
RUN dotnet publish WineShop.csproj -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

# App listens on port 8080 internally
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "WineShop.dll"]
