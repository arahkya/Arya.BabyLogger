FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["Arya.BabyLogger.WebApi/Arya.BabyLogger.WebApi.csproj", "Arya.BabyLogger.WebApi/"]
RUN dotnet restore "Arya.BabyLogger.WebApi/Arya.BabyLogger.WebApi.csproj"
COPY . .
WORKDIR "/src/Arya.BabyLogger.WebApi"
RUN dotnet build "./Arya.BabyLogger.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Arya.BabyLogger.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Arya.BabyLogger.WebApi.dll"]