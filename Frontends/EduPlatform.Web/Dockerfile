FROM mcr.microsoft.com/dotnet/sdk:6.0-buster-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/EduPlatform.Shared/*.csproj Shared/EduPlatform.Shared/
COPY Frontends/EduPlatform.Web/*.csproj Frontends/EduPlatform.Web/
RUN dotnet restore Frontends/EduPlatform.Web/*.csproj
COPY . .
RUN dotnet publish Frontends/EduPlatform.Web/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:6.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","EduPlatform.Web.dll" ]