#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/StateMachines/SagaOrchestrationStateMachine/SagaOrchestrationStateMachine.csproj", "src/StateMachines/SagaOrchestrationStateMachine/"]
COPY ["src/BuildingBlocks/EventBus/EventBus.csproj", "src/BuildingBlocks/EventBus/"]
COPY ["src/BuildingBlocks/Logging/Logging.csproj", "src/BuildingBlocks/Logging/"]
RUN dotnet restore "src/StateMachines/SagaOrchestrationStateMachine/SagaOrchestrationStateMachine.csproj"
COPY . .
WORKDIR "/src/src/StateMachines/SagaOrchestrationStateMachine"
RUN dotnet build "SagaOrchestrationStateMachine.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SagaOrchestrationStateMachine.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SagaOrchestrationStateMachine.dll"]