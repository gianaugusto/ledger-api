FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/LedgerAPI/LedgerAPI.csproj", "LedgerAPI/"]
COPY ["tests/LedgerAPI.Tests/LedgerAPI.Tests.csproj", "tests/LedgerAPI.Tests/"]
WORKDIR "/src/LedgerAPI"
RUN dotnet restore "LedgerAPI.csproj"
COPY . .
WORKDIR "/src/LedgerAPI"
RUN dotnet build "LedgerAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LedgerAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LedgerAPI.dll"]
