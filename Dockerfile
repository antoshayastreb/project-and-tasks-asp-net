FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/ProjectTask.Api/ProjectTask.Api.csproj", "src/ProjectTask.Api/"]
COPY ["src/ProjectTask.Application/ProjectTask.Application.csproj", "src/ProjectTask.Application/"]
COPY ["src/ProjectTask.Domain/ProjectTask.Domain.csproj", "src/ProjectTask.Domain/"]
COPY ["src/ProjectTask.Infrastructure/ProjectTask.Infrastructure.csproj", "src/ProjectTask.Infrastructure/"]
RUN dotnet restore "src/ProjectTask.Api/ProjectTask.Api.csproj"

COPY . .
RUN dotnet publish "src/ProjectTask.Api/ProjectTask.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "ProjectTask.Api.dll"]