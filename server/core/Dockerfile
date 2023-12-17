FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["server/core/core.csproj", "server/core/"]
COPY ["server/shared/shared.csproj", "server/shared/"]
RUN dotnet restore "server/core/core.csproj"
COPY ["client/Assets/Sample/Scripts/Server/Shared/", "client/Assets/Sample/Scripts/Server/Shared/"]
COPY ["server/core/", "server/core/"]
COPY ["server/shared/", "server/shared/"]
WORKDIR "/src/server/core"
RUN dotnet build "core.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "core.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "core.dll"]
