FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Workouts/Workouts.Api/Workouts.Api.csproj", "Workouts/Workouts.Api/"]
COPY ["Workouts/Workouts.Core/Workouts.Core.csproj", "Workouts/Workouts.Core/"]
COPY ["Workouts/Workouts.Infrastructure/Workouts.Infrastructure.csproj", "Workouts/Workouts.Infrastructure/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["NoDaysOff.ServiceDefaults/NoDaysOff.ServiceDefaults.csproj", "NoDaysOff.ServiceDefaults/"]
RUN dotnet restore "Workouts/Workouts.Api/Workouts.Api.csproj"
COPY . .
WORKDIR "/src/Workouts/Workouts.Api"
RUN dotnet build "Workouts.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Workouts.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Workouts.Api.dll"]
