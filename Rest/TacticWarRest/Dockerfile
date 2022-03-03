#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Rest/TacticWarRest/TacticWar.Rest.csproj", "Rest/TacticWarRest/"]
COPY ["Lib/TacticWarLib/TacticWar.Lib.csproj", "Lib/TacticWarLib/"]
COPY ["DependencyInjection/TacticWar.Lib.Extensions.Microsoft.DependencyInjection/TacticWar.DependencyInjection.csproj", "DependencyInjection/TacticWar.Lib.Extensions.Microsoft.DependencyInjection/"]
RUN dotnet restore "Rest/TacticWarRest/TacticWar.Rest.csproj"
COPY . .
WORKDIR "/src/Rest/TacticWarRest"
RUN dotnet build "TacticWar.Rest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TacticWar.Rest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TacticWar.Rest.dll"]