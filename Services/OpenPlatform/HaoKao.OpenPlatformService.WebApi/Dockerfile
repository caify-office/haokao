# 请参阅 https://aka.ms/customizecontainer 以了解如何自定义调试容器，以及 Visual Studio 如何使用此 Dockerfile 生成映像以更快地进行调试。

# 此阶段用于在快速模式(默认为调试配置)下从 VS 运行时
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
# USER $APP_UID
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# 此阶段用于生成服务项目
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/OpenPlatform/HaoKao.OpenPlatformService.WebApi/HaoKao.OpenPlatformService.WebApi.csproj", "Services/OpenPlatform/HaoKao.OpenPlatformService.WebApi/"]
COPY ["Services/OpenPlatform/HaoKao.OpenPlatformService.Application/HaoKao.OpenPlatformService.Application.csproj", "Services/OpenPlatform/HaoKao.OpenPlatformService.Application/"]
COPY ["Services/OpenPlatform/HaoKao.OpenPlatformService.Domain/HaoKao.OpenPlatformService.Domain.csproj", "Services/OpenPlatform/HaoKao.OpenPlatformService.Domain/"]
COPY ["HaoKao.Common/HaoKao.Common.csproj", "HaoKao.Common/"]
COPY ["Services/OpenPlatform/HaoKao.OpenPlatformService.Infrastructure/HaoKao.OpenPlatformService.Infrastructure.csproj", "Services/OpenPlatform/HaoKao.OpenPlatformService.Infrastructure/"]
RUN dotnet restore "./Services/OpenPlatform/HaoKao.OpenPlatformService.WebApi/HaoKao.OpenPlatformService.WebApi.csproj"
COPY . .
WORKDIR "/src/Services/OpenPlatform/HaoKao.OpenPlatformService.WebApi"
RUN dotnet build "./HaoKao.OpenPlatformService.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 此阶段用于发布要复制到最终阶段的服务项目
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HaoKao.OpenPlatformService.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 此阶段在生产中使用，或在常规模式下从 VS 运行时使用(在不使用调试配置时为默认值)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime
ENTRYPOINT ["dotnet", "HaoKao.OpenPlatformService.WebApi.dll"]