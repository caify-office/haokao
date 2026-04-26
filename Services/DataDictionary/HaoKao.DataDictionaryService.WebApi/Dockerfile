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
COPY ["Services/DataDictionary/HaoKao.DataDictionaryService.WebApi/HaoKao.DataDictionaryService.WebApi.csproj", "Services/DataDictionary/HaoKao.DataDictionaryService.WebApi/"]
COPY ["Services/DataDictionary/HaoKao.DataDictionaryService.Application/HaoKao.DataDictionaryService.Application.csproj", "Services/DataDictionary/HaoKao.DataDictionaryService.Application/"]
COPY ["Services/DataDictionary/HaoKao.DataDictionaryService.Domain/HaoKao.DataDictionaryService.Domain.csproj", "Services/DataDictionary/HaoKao.DataDictionaryService.Domain/"]
COPY ["HaoKao.Common/HaoKao.Common.csproj", "HaoKao.Common/"]
COPY ["Services/DataDictionary/HaoKao.DataDictionaryService.Infrastructure/HaoKao.DataDictionaryService.Infrastructure.csproj", "Services/DataDictionary/HaoKao.DataDictionaryService.Infrastructure/"]
RUN dotnet restore "./Services/DataDictionary/HaoKao.DataDictionaryService.WebApi/HaoKao.DataDictionaryService.WebApi.csproj"
COPY . .
WORKDIR "/src/Services/DataDictionary/HaoKao.DataDictionaryService.WebApi"
RUN dotnet build "./HaoKao.DataDictionaryService.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 此阶段用于发布要复制到最终阶段的服务项目
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HaoKao.DataDictionaryService.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 此阶段在生产中使用，或在常规模式下从 VS 运行时使用(在不使用调试配置时为默认值)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime
ENTRYPOINT ["dotnet", "HaoKao.DataDictionaryService.WebApi.dll"]