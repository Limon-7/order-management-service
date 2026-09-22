# See https://aka.ms/customizecontainer to learn how to customize your debug container.

FROM docker.bracits.com/mcr/dotnet/runtime:10.0 AS base
WORKDIR /app

FROM docker.bracits.com/mcr/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/Read/EventWorker/EventWorker.csproj",             "src/Read/EventWorker/"]
COPY ["src/Read/EventHandlers/EventHandlers.csproj",         "src/Read/EventHandlers/"]
COPY ["src/Read/ViewModels/ViewModels.csproj",               "src/Read/ViewModels/"]
COPY ["src/Domain/Events/Events.csproj",                     "src/Domain/Events/"]
COPY ["src/Domain/Models/Models.csproj",                     "src/Domain/Models/"]
COPY ["src/Shared/Common/Common.csproj",                     "src/Shared/Common/"]
COPY ["src/Shared/SharedDto/SharedDto.csproj",               "src/Shared/SharedDto/"]
COPY ["Directory.Packages.props",                            "."]
COPY ["Directory.Build.props",                               "."]

RUN mkdir -p /root/.nuget/NuGet
COPY ./config/NuGetPackageSource.Config /root/.nuget/NuGet/NuGet.Config

RUN dotnet restore "src/Read/EventWorker/EventWorker.csproj"
COPY . .
WORKDIR "/src/src/Read/EventWorker"
RUN dotnet build "EventWorker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EventWorker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bits.OrderManagement.Read.EventWorker.dll"]
