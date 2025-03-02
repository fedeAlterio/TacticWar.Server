FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish Rest/TacticWarRest/TacticWar.Rest.csproj -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build /App/out .
ENTRYPOINT ["dotnet", "TacticWar.Rest.dll"]