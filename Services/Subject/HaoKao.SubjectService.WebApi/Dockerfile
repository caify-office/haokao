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
COPY ["Services/Subject/HaoKao.SubjectService.WebApi/HaoKao.SubjectService.WebApi.csproj", "Services/Subject/HaoKao.SubjectService.WebApi/"]
COPY ["Services/Subject/HaoKao.SubjectService.Application/HaoKao.SubjectService.Application.csproj", "Services/Subject/HaoKao.SubjectService.Application/"]
COPY ["Services/Subject/HaoKao.SubjectService.Domian/HaoKao.SubjectService.Domain.csproj", "Services/Subject/HaoKao.SubjectService.Domian/"]
COPY ["HaoKao.Common/HaoKao.Common.csproj", "HaoKao.Common/"]
COPY ["Services/Subject/HaoKao.SubjectService.Infrastucture/HaoKao.SubjectService.Infrastructure.csproj", "Services/Subject/HaoKao.SubjectService.Infrastucture/"]
RUN dotnet restore "./Services/Subject/HaoKao.SubjectService.WebApi/HaoKao.SubjectService.WebApi.csproj"
COPY . .
WORKDIR "/src/Services/Subject/HaoKao.SubjectService.WebApi"
RUN dotnet build "./HaoKao.SubjectService.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 此阶段用于发布要复制到最终阶段的服务项目
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HaoKao.SubjectService.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 此阶段在生产中使用，或在常规模式下从 VS 运行时使用(在不使用调试配置时为默认值)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime
ENTRYPOINT ["dotnet", "HaoKao.SubjectService.WebApi.dll"]