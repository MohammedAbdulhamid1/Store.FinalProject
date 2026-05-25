# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY Store.FinalProject.sln ./
COPY Store.FinalProject/Store.FinalProject.csproj Store.FinalProject/
COPY Store.Core/Store.Core.csproj Store.Core/
COPY Store.Repository/Store.Repository.csproj Store.Repository/
COPY Store.Service/Store.Service.csproj Store.Service/

# Restore dependencies
RUN dotnet restore Store.FinalProject.sln

# Copy everything else
COPY . .

# Build & Publish
RUN dotnet publish Store.FinalProject/Store.FinalProject.csproj -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

# Render injects PORT env variable
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}

ENTRYPOINT ["dotnet", "Store.FinalProject.dll"]
