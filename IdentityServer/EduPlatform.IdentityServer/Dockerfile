FROM mcr.microsoft.com/dotnet/sdk:3.1 as build
WORKDIR /app
EXPOSE 80
COPY Shared/EduPlatform.Shared/*.csproj Shared/EduPlatform.Shared/
COPY IdentityServer/EduPlatform.IdentityServer/*.csproj IdentityServer/EduPlatform.IdentityServer/ 
RUN dotnet restore  IdentityServer/EduPlatform.IdentityServer/*.csproj
COPY . .
RUN dotnet publish IdentityServer/EduPlatform.IdentityServer/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:3.1 as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","EduPlatform.IdentityServer.dll" ]