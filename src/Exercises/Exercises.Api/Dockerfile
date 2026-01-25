FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Exercises/Exercises.Api/Exercises.Api.csproj", "Exercises/Exercises.Api/"]
COPY ["Exercises/Exercises.Core/Exercises.Core.csproj", "Exercises/Exercises.Core/"]
COPY ["Exercises/Exercises.Infrastructure/Exercises.Infrastructure.csproj", "Exercises/Exercises.Infrastructure/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["NoDaysOff.ServiceDefaults/NoDaysOff.ServiceDefaults.csproj", "NoDaysOff.ServiceDefaults/"]
RUN dotnet restore "Exercises/Exercises.Api/Exercises.Api.csproj"
COPY . .
WORKDIR "/src/Exercises/Exercises.Api"
RUN dotnet build "Exercises.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Exercises.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Exercises.Api.dll"]
