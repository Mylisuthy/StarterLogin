# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY src/StarterLogin.Api/*.csproj ./src/StarterLogin.Api/
COPY src/StarterLogin.Application/*.csproj ./src/StarterLogin.Application/
COPY src/StarterLogin.Domain/*.csproj ./src/StarterLogin.Domain/
COPY src/StarterLogin.Infrastructure/*.csproj ./src/StarterLogin.Infrastructure/
RUN dotnet restore src/StarterLogin.Api/StarterLogin.Api.csproj

# Copy everything else and build
COPY . .
RUN dotnet publish src/StarterLogin.Api/StarterLogin.Api.csproj -c Release -o out

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# ENV variables for SQL Server connection
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "StarterLogin.Api.dll"]
