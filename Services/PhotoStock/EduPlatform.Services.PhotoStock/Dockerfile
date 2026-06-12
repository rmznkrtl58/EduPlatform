FROM mcr.microsoft.com/dotnet/sdk:6.0-buster-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/EduPlatform.Shared/*.csproj Shared/EduPlatform.Shared/
COPY Services/PhotoStock/EduPlatform.Services.PhotoStock/*.csproj Services/PhotoStock/EduPlatform.Services.PhotoStock/
RUN dotnet restore Services/PhotoStock/EduPlatform.Services.PhotoStock/*.csproj
COPY . .
RUN dotnet publish Services/PhotoStock/EduPlatform.Services.PhotoStock/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:6.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","EduPlatform.Services.PhotoStock.dll" ]