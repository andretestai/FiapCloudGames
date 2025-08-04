# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FiapCloudGames/FiapCloudGames.csproj", "FiapCloudGames/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["Services/Services.csproj", "Services/"]
RUN dotnet restore "./FiapCloudGames/FiapCloudGames.csproj"
COPY . .
WORKDIR "/src/FiapCloudGames"
RUN dotnet build "./FiapCloudGames.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FiapCloudGames.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final

# Instala o agente New Relic
USER root
RUN apt-get update && apt-get install -y wget ca-certificates gnupg \
 && echo 'deb [signed-by=/usr/share/keyrings/newrelic-apt.gpg] http://apt.newrelic.com/debian/ newrelic non-free' | tee /etc/apt/sources.list.d/newrelic.list \
 && wget -O- https://download.newrelic.com/NEWRELIC_APT_2DAD550E.public | gpg --import --batch --no-default-keyring --keyring /usr/share/keyrings/newrelic-apt.gpg \
 && apt-get update \
 && apt-get install -y newrelic-dotnet-agent \
 && rm -rf /var/lib/apt/lists/*

# Variáveis de ambiente para ativar o agente
ENV CORECLR_ENABLE_PROFILING=1 \
    CORECLR_PROFILER="{36032161-FFC0-4B61-B559-F6C5D41BAE5A}" \
    CORECLR_NEWRELIC_HOME="/usr/local/newrelic-dotnet-agent" \
    CORECLR_PROFILER_PATH="/usr/local/newrelic-dotnet-agent/libNewRelicProfiler.so"

# Retorna ao usuário da aplicação
USER $APP_UID

WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "FiapCloudGames.dll"]
