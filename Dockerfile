# =========================
# Base runtime
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Porta HTTP e HTTPS
EXPOSE 8080
EXPOSE 8443

# =========================
# Build
# =========================
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

# =========================
# Publish
# =========================
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FiapCloudGames.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

# =========================
# Final image
# =========================
FROM base AS final

# 🔥 New Relic PRECISA rodar como root
USER root

RUN apt-get update && apt-get install -y wget ca-certificates gnupg \
 && echo 'deb [signed-by=/usr/share/keyrings/newrelic-apt.gpg] http://apt.newrelic.com/debian/ newrelic non-free' \
    | tee /etc/apt/sources.list.d/newrelic.list \
 && wget -O- https://download.newrelic.com/NEWRELIC_APT_2DAD550E.public \
    | gpg --import --batch --no-default-keyring --keyring /usr/share/keyrings/newrelic-apt.gpg \
 && apt-get update \
 && apt-get install -y newrelic-dotnet-agent \
 && rm -rf /var/lib/apt/lists/*

# =========================
# New Relic ENV
# =========================
ENV CORECLR_ENABLE_PROFILING=1 \
    CORECLR_PROFILER="{36032161-FFC0-4B61-B559-F6C5D41BAE5A}" \
    CORECLR_NEWRELIC_HOME="/usr/local/newrelic-dotnet-agent" \
    CORECLR_PROFILER_PATH="/usr/local/newrelic-dotnet-agent/libNewRelicProfiler.so"

# =========================
# App
# =========================
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "FiapCloudGames.dll"]
