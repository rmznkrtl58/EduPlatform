FROM mcr.microsoft.com/dotnet/sdk:6.0-buster-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/EduPlatform.Shared/*.csproj Shared/EduPlatform.Shared/
COPY Services/Basket/EduPlatform.Services.Basket/*.csproj Services/Basket/EduPlatform.Services.Basket/
RUN dotnet restore Services/Basket/EduPlatform.Services.Basket/*.csproj
COPY . .
RUN dotnet publish Services/Basket/EduPlatform.Services.Basket/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:6.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","EduPlatform.Services.Basket.dll" ]