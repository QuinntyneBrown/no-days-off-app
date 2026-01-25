FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Athletes/Athletes.Api/Athletes.Api.csproj", "Athletes/Athletes.Api/"]
COPY ["Athletes/Athletes.Core/Athletes.Core.csproj", "Athletes/Athletes.Core/"]
COPY ["Athletes/Athletes.Infrastructure/Athletes.Infrastructure.csproj", "Athletes/Athletes.Infrastructure/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["NoDaysOff.ServiceDefaults/NoDaysOff.ServiceDefaults.csproj", "NoDaysOff.ServiceDefaults/"]
RUN dotnet restore "Athletes/Athletes.Api/Athletes.Api.csproj"
COPY . .
WORKDIR "/src/Athletes/Athletes.Api"
RUN dotnet build "Athletes.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Athletes.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Athletes.Api.dll"]
