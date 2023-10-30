
# Use the official .NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

# Use the official .NET Core SDK as a parent image for building
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR Shop
COPY ["Shop.API.csproj", "."]
RUN dotnet restore "Shop.API.csproj"
COPY . .
WORKDIR "/Shop"
RUN dotnet build "Shop.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Shop.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shop.API.dll"]
