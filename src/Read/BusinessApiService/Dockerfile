# See https://aka.ms/customizecontainer to learn how to customize your debug container.

FROM docker.bracits.com/mcr/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM docker.bracits.com/mcr/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/Read/BusinessApiService/BusinessApiService.csproj",   "src/Read/BusinessApiService/"]
COPY ["src/Read/QueryHandlers/QueryHandlers.csproj",             "src/Read/QueryHandlers/"]
COPY ["src/Read/ViewModels/ViewModels.csproj",                   "src/Read/ViewModels/"]
COPY ["src/Domain/Events/Events.csproj",                         "src/Domain/Events/"]
COPY ["src/Domain/Models/Models.csproj",                         "src/Domain/Models/"]
COPY ["src/Shared/Common/Common.csproj",                         "src/Shared/Common/"]
COPY ["src/Shared/SharedDto/SharedDto.csproj",                   "src/Shared/SharedDto/"]
COPY ["Directory.Packages.props",                                "."]
COPY ["Directory.Build.props",                                   "."]

RUN mkdir -p /root/.nuget/NuGet
COPY ./config/NuGetPackageSource.Config /root/.nuget/NuGet/NuGet.Config

RUN dotnet restore "src/Read/BusinessApiService/BusinessApiService.csproj"
COPY . .
WORKDIR "/src/src/Read/BusinessApiService"
RUN dotnet build "BusinessApiService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BusinessApiService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bits.OrderManagement.Read.BusinessApiService.dll"]
