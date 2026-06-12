FROM mcr.microsoft.com/dotnet/sdk:6.0-buster-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/EduPlatform.Shared/*.csproj Shared/EduPlatform.Shared/
COPY Services/Catalog/EduPlatform.Services.Catalog/*.csproj Services/Catalog/EduPlatform.Services.Catalog/
RUN dotnet restore Services/Catalog/EduPlatform.Services.Catalog/*.csproj
COPY . .
RUN dotnet publish Services/Catalog/EduPlatform.Services.Catalog/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:6.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","EduPlatform.Services.Catalog.dll" ]