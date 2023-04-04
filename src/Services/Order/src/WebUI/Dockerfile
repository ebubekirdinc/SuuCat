#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/Order/src/WebUI/WebUI.csproj", "src/Services/Order/src/WebUI/"]
COPY ["src/BuildingBlocks/Logging/Logging.csproj", "src/BuildingBlocks/Logging/"]
COPY ["src/Services/Order/src/Application/Application.csproj", "src/Services/Order/src/Application/"]
COPY ["src/BuildingBlocks/EventBus/EventBus.csproj", "src/BuildingBlocks/EventBus/"]
COPY ["src/BuildingBlocks/Shared/Shared.csproj", "src/BuildingBlocks/Shared/"]
COPY ["src/Services/Order/src/Domain/Domain.csproj", "src/Services/Order/src/Domain/"]
COPY ["src/Services/Order/src/Infrastructure/Infrastructure.csproj", "src/Services/Order/src/Infrastructure/"]
RUN dotnet restore "src/Services/Order/src/WebUI/WebUI.csproj"
COPY . .
WORKDIR "/src/src/Services/Order/src/WebUI"
RUN dotnet build "WebUI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebUI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebUI.dll"]