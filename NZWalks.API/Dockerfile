# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY NZWalks.API/NZWalks.API.csproj NZWalks.API/
RUN dotnet restore NZWalks.API/NZWalks.API.csproj

COPY . .
RUN dotnet publish NZWalks.API/NZWalks.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# ASP.NET Core listens on 8080 in containers by default (recommended)
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "NZWalks.API.dll"]