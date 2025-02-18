# Use official .NET 9 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["CartService.csproj", "./"]
RUN dotnet restore "CartService.csproj"
COPY . .
RUN dotnet build "CartService.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "CartService.csproj" -c Release -o /app/publish

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CartService.dll"]
