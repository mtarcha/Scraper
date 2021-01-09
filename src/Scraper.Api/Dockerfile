#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/Scraper.Api/Scraper.Api.csproj", "src/Scraper.Api/"]
COPY ["src/Scraper.Domain/Scraper.Domain.csproj", "src/Scraper.Domain/"]
COPY ["src/Scraper.Infrastructure.Mongo/Scraper.Infrastructure.Mongo.csproj", "src/Scraper.Infrastructure.Mongo/"]
RUN dotnet restore "src/Scraper.Api/Scraper.Api.csproj"
COPY . .
WORKDIR "/src/src/Scraper.Api"
RUN dotnet build "Scraper.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Scraper.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Scraper.Api.dll"]
