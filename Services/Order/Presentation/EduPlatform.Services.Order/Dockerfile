FROM mcr.microsoft.com/dotnet/sdk:6.0-buster-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/EduPlatform.Shared/*.csproj Shared/EduPlatform.Shared/

COPY Services/Order/Core/EduPlatform.Services.Order.Application/*.csproj Services/Order/EduPlatform.Services.Order.Application/

COPY Services/Order/Core/EduPlatform.Services.Order.Domain/*.csproj Services/Order/EduPlatform.Services.Order.Domain/

COPY Services/Order/External/EduPlatform.Services.Order.Infrastructure/*.csproj Services/Order/EduPlatform.Services.Order.Infrastructure/


COPY Services/Order/Presentation/EduPlatform.Services.Order/*.csproj Services/Order/EduPlatform.Services.Order/
RUN dotnet restore Services/Order/Presentation/EduPlatform.Services.Order/*.csproj
COPY . .
RUN dotnet publish Services/Order/Presentation/EduPlatform.Services.Order/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:6.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","EduPlatform.Services.Order.dll" ]