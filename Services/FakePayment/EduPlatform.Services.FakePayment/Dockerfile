FROM mcr.microsoft.com/dotnet/sdk:6.0-buster-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/EduPlatform.Shared/*.csproj Shared/EduPlatform.Shared/
COPY Services/FakePayment/EduPlatform.Services.FakePayment/*.csproj Services/FakePayment/EduPlatform.Services.FakePayment/
RUN dotnet restore Services/FakePayment/EduPlatform.Services.FakePayment/*.csproj
COPY . .
RUN dotnet publish Services/FakePayment/EduPlatform.Services.FakePayment/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:6.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","EduPlatform.Services.FakePayment.dll" ]