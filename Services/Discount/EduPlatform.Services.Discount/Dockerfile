FROM mcr.microsoft.com/dotnet/sdk:6.0-buster-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/EduPlatform.Shared/*.csproj Shared/EduPlatform.Shared/
COPY Services/Discount/EduPlatform.Services.Discount/*.csproj Services/Discount/EduPlatform.Services.Discount/
RUN dotnet restore Services/Discount/EduPlatform.Services.Discount/*.csproj
COPY . .
RUN dotnet publish Services/Discount/EduPlatform.Services.Discount/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:6.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","EduPlatform.Services.Discount.dll" ]