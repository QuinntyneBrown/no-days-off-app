# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

# Copy solution and project files
COPY NoDaysOff.sln .
COPY src/NoDaysOff.Services.Athletes/*.csproj ./src/NoDaysOff.Services.Athletes/
COPY src/NoDaysOff.Core/*.csproj ./src/NoDaysOff.Core/
COPY src/NoDaysOff.Infrastructure/*.csproj ./src/NoDaysOff.Infrastructure/
COPY src/NoDaysOff.Shared/*.csproj ./src/NoDaysOff.Shared/

# Restore dependencies
RUN dotnet restore src/NoDaysOff.Services.Athletes/NoDaysOff.Services.Athletes.csproj

# Copy all source files
COPY src/ ./src/

# Build and publish
WORKDIR /source/src/NoDaysOff.Services.Athletes
RUN dotnet publish -c Release -o /app --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .

# Create a non-root user
RUN useradd -m -s /bin/bash appuser && chown -R appuser:appuser /app
USER appuser

EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "NoDaysOff.Services.Athletes.dll"]
