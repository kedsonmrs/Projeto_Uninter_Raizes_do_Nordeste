FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["RaizesDoNordeste.API/RaizesDoNordeste.API.csproj", "RaizesDoNordeste.API/"]
COPY ["RaizesDoNordeste.Application/RaizesDoNordeste.Application.csproj", "RaizesDoNordeste.Application/"]
COPY ["RaizesDoNordeste.Domain/RaizesDoNordeste.Domain.csproj", "RaizesDoNordeste.Domain/"]
COPY ["RaizesDoNordeste.Infra/RaizesDoNordeste.Infrastructure.csproj", "RaizesDoNordeste.Infra/"]

RUN dotnet restore "RaizesDoNordeste.API/RaizesDoNordeste.API.csproj"

COPY . .
WORKDIR "/src/RaizesDoNordeste.API"
RUN dotnet build "RaizesDoNordeste.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RaizesDoNordeste.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RaizesDoNordeste.API.dll"]