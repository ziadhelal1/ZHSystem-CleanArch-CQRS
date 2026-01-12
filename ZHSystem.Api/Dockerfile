# 1. مرحلة البناء (Build)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src


COPY ["School.Api/School.Api.csproj", "School.Api/"]
COPY ["School.Application/School.Application.csproj", "School.Application/"]
COPY ["School.Infrastructure/School.Infrastructure.csproj", "School.Infrastructure/"]
COPY ["School.Domain/School.Domain.csproj", "School.Domain/"]

RUN dotnet restore "School.Api/School.Api.csproj"


COPY . .
WORKDIR "/src/School.Api"
RUN dotnet build "School.Api.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "School.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "School.Api.dll"]