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
COPY ["Services/Paper/HaoKao.PaperService.WebApi/HaoKao.PaperService.WebApi.csproj", "Services/Paper/HaoKao.PaperService.WebApi/"]
COPY ["Services/Paper/HaoKao.PaperService.Application/HaoKao.PaperService.Application.csproj", "Services/Paper/HaoKao.PaperService.Application/"]
COPY ["Services/Paper/HaoKao.PaperService.Domain/HaoKao.PaperService.Domain.csproj", "Services/Paper/HaoKao.PaperService.Domain/"]
COPY ["HaoKao.Common/HaoKao.Common.csproj", "HaoKao.Common/"]
COPY ["Services/Paper/HaoKao.PaperService.Infrastructure/HaoKao.PaperService.Infrastructure.csproj", "Services/Paper/HaoKao.PaperService.Infrastructure/"]
RUN dotnet restore "./Services/Paper/HaoKao.PaperService.WebApi/HaoKao.PaperService.WebApi.csproj"
COPY . .
WORKDIR "/src/Services/Paper/HaoKao.PaperService.WebApi"
RUN dotnet build "./HaoKao.PaperService.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 此阶段用于发布要复制到最终阶段的服务项目
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HaoKao.PaperService.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 此阶段在生产中使用，或在常规模式下从 VS 运行时使用(在不使用调试配置时为默认值)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime
ENTRYPOINT ["dotnet", "HaoKao.PaperService.WebApi.dll"]