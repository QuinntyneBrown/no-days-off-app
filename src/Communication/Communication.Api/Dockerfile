FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Communication/Communication.Api/Communication.Api.csproj", "Communication/Communication.Api/"]
COPY ["Communication/Communication.Core/Communication.Core.csproj", "Communication/Communication.Core/"]
COPY ["Communication/Communication.Infrastructure/Communication.Infrastructure.csproj", "Communication/Communication.Infrastructure/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["NoDaysOff.ServiceDefaults/NoDaysOff.ServiceDefaults.csproj", "NoDaysOff.ServiceDefaults/"]
RUN dotnet restore "Communication/Communication.Api/Communication.Api.csproj"
COPY . .
WORKDIR "/src/Communication/Communication.Api"
RUN dotnet build "Communication.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Communication.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Communication.Api.dll"]
