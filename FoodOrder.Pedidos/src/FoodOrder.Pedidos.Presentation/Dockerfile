# Imagem base para runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Imagem para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia os projetos individualmente para otimizar cache de build
COPY ["src/FoodOrder.Pedidos.Presentation/FoodOrder.Pedidos.Presentation.csproj", "src/FoodOrder.Pedidos.Presentation/"]
COPY ["src/FoodOrder.Pedidos.Application/FoodOrder.Pedidos.Application.csproj", "src/FoodOrder.Pedidos.Application/"]
COPY ["src/FoodOrder.Pedidos.Domain/FoodOrder.Pedidos.Domain.csproj", "src/FoodOrder.Pedidos.Domain/"]
COPY ["src/FoodOrder.Pedidos.Infrastructure/FoodOrder.Pedidos.Infrastructure.csproj", "src/FoodOrder.Pedidos.Infrastructure/"]
RUN dotnet restore "src/FoodOrder.Pedidos.Presentation/FoodOrder.Pedidos.Presentation.csproj"

# Copia o restante dos arquivos
COPY . .
WORKDIR "/src/src/FoodOrder.Pedidos.Presentation"
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

# Publica a aplicação
FROM build AS publish
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Imagem final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FoodOrder.Pedidos.Presentation.dll"]
