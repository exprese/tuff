FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Modding.csproj", "."]
RUN dotnet restore "Modding.csproj"
COPY . .
RUN dotnet build "Modding.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Modding.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Modding.dll"]
