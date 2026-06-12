FROM mcr.microsoft.com/dotnet/sdk:6.0-buster-slim as build
WORKDIR /app
EXPOSE 80

COPY  Gateways/EduPlatform.Gateway/*.csproj  EduPlatform.Gateway/
RUN dotnet restore Gateways/EduPlatform.Gateway/*.csproj
COPY . .
RUN dotnet publish Gateways/EduPlatform.Gateway/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:6.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","EduPlatform.Gateway.dll" ]