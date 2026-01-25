FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Dashboard/Dashboard.Api/Dashboard.Api.csproj", "Dashboard/Dashboard.Api/"]
COPY ["Dashboard/Dashboard.Core/Dashboard.Core.csproj", "Dashboard/Dashboard.Core/"]
COPY ["Dashboard/Dashboard.Infrastructure/Dashboard.Infrastructure.csproj", "Dashboard/Dashboard.Infrastructure/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["NoDaysOff.ServiceDefaults/NoDaysOff.ServiceDefaults.csproj", "NoDaysOff.ServiceDefaults/"]
RUN dotnet restore "Dashboard/Dashboard.Api/Dashboard.Api.csproj"
COPY . .
WORKDIR "/src/Dashboard/Dashboard.Api"
RUN dotnet build "Dashboard.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dashboard.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dashboard.Api.dll"]
