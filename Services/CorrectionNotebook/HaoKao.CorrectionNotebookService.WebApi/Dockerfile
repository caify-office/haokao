#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["HaoKao.CorrectionNotebookService.Domain/HaoKao.CorrectionNotebookService.Domain.csproj", "HaoKao.CorrectionNotebookService.Domain/"]
COPY ["HaoKao.CorrectionNotebookService.Application/HaoKao.CorrectionNotebookService.Application.csproj", "HaoKao.CorrectionNotebookService.Application/"]
COPY ["HaoKao.CorrectionNotebookService.Infrastructure/HaoKao.CorrectionNotebookService.Infrastructure.csproj", "HaoKao.CorrectionNotebookService.Infrastructure/"]
COPY ["HaoKao.CorrectionNotebookService.WebApi/HaoKao.CorrectionNotebookService.WebApi.csproj", "HaoKao.CorrectionNotebookService.WebApi/"]
RUN dotnet restore "HaoKao.CorrectionNotebookService.WebApi/HaoKao.CorrectionNotebookService.WebApi.csproj"
COPY . .
WORKDIR "/src/HaoKao.CorrectionNotebookService.WebApi"
RUN dotnet build "HaoKao.CorrectionNotebookService.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HaoKao.CorrectionNotebookService.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime
ENTRYPOINT ["dotnet", "HaoKao.CorrectionNotebookService.WebApi.dll"]