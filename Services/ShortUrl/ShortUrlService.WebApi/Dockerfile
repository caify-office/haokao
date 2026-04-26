#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ShortUrlService.WebApi/ShortUrlService.WebApi.csproj", "ShortUrlService.WebApi/"]
COPY ["ShortUrlService.Domain/ShortUrlService.Domain.csproj", "ShortUrlService.Domain/"]
COPY ["ShortUrlService.Infrastructure/ShortUrlService.Infrastructure.csproj", "ShortUrlService.Infrastructure/"]
RUN dotnet restore "./ShortUrlService.WebApi/ShortUrlService.WebApi.csproj"
COPY . .
WORKDIR "/src/ShortUrlService.WebApi"
RUN dotnet build "./ShortUrlService.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ShortUrlService.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime 
ENTRYPOINT ["dotnet", "ShortUrlService.WebApi.dll"]