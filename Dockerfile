#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["FoodRatingApi.csproj", "."]
RUN dotnet restore "./FoodRatingApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "FoodRatingApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FoodRatingApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FoodRatingApi.dll"]