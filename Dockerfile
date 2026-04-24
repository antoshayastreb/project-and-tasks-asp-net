FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/ProjectManager.Api/ProjectManager.Api.csproj", "src/ProjectManager.Api/"]
COPY ["src/ProjectManager.Application/ProjectManager.Application.csproj", "src/ProjectManager.Application/"]
COPY ["src/ProjectManager.Domain/ProjectManager.Domain.csproj", "src/ProjectManager.Domain/"]
COPY ["src/ProjectManager.Infrastructure/ProjectManager.Infrastructure.csproj", "src/ProjectManager.Infrastructure/"]
RUN dotnet restore "src/ProjectManager.Api/ProjectManager.Api.csproj"

COPY . .
RUN dotnet publish "src/ProjectManager.Api/ProjectManager.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "ProjectManager.Api.dll"]