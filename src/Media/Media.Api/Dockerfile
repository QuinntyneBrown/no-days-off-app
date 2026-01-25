FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Media/Media.Api/Media.Api.csproj", "Media/Media.Api/"]
COPY ["Media/Media.Core/Media.Core.csproj", "Media/Media.Core/"]
COPY ["Media/Media.Infrastructure/Media.Infrastructure.csproj", "Media/Media.Infrastructure/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["NoDaysOff.ServiceDefaults/NoDaysOff.ServiceDefaults.csproj", "NoDaysOff.ServiceDefaults/"]
RUN dotnet restore "Media/Media.Api/Media.Api.csproj"
COPY . .
WORKDIR "/src/Media/Media.Api"
RUN dotnet build "Media.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Media.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Media.Api.dll"]
