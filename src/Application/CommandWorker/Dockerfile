# See https://aka.ms/customizecontainer to learn how to customize your debug container.

FROM docker.bracits.com/mcr/dotnet/runtime:10.0 AS base
WORKDIR /app

FROM docker.bracits.com/mcr/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/Application/CommandWorker/CommandWorker.csproj",         "src/Application/CommandWorker/"]
COPY ["src/Application/CommandHandlers/CommandHandlers.csproj",     "src/Application/CommandHandlers/"]
COPY ["src/Application/CommandServices/CommandServices.csproj",     "src/Application/CommandServices/"]
COPY ["src/Application/Commands/Commands.csproj",                   "src/Application/Commands/"]
COPY ["src/Application/DataMappers/DataMappers.csproj",             "src/Application/DataMappers/"]
COPY ["src/Domain/Aggregates/Aggregates.csproj",                    "src/Domain/Aggregates/"]
COPY ["src/Domain/DomainServices/DomainServices.csproj",            "src/Domain/DomainServices/"]
COPY ["src/Domain/Entities/Entities.csproj",                        "src/Domain/Entities/"]
COPY ["src/Domain/Events/Events.csproj",                            "src/Domain/Events/"]
COPY ["src/Domain/Models/Models.csproj",                            "src/Domain/Models/"]
COPY ["src/Domain/ValueObjects/ValueObjects.csproj",                "src/Domain/ValueObjects/"]
COPY ["src/Infrastructure/Infrastructure/Infrastructure.csproj",    "src/Infrastructure/Infrastructure/"]
COPY ["src/Read/ViewModels/ViewModels.csproj",                      "src/Read/ViewModels/"]
COPY ["src/Shared/Common/Common.csproj",                            "src/Shared/Common/"]
COPY ["src/Shared/SharedDto/SharedDto.csproj",                      "src/Shared/SharedDto/"]
COPY ["Directory.Packages.props",                                    "."]
COPY ["Directory.Build.props",                                       "."]

RUN mkdir -p /root/.nuget/NuGet
COPY ./config/NuGetPackageSource.Config /root/.nuget/NuGet/NuGet.Config

RUN dotnet restore "src/Application/CommandWorker/CommandWorker.csproj"
COPY . .
WORKDIR "/src/src/Application/CommandWorker"
RUN dotnet build "CommandWorker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CommandWorker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bits.OrderManagement.Application.CommandWorker.dll"]
