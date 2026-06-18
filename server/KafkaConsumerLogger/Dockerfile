# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore only the dependencies
COPY ["KafkaConsumerLogger/KafkaConsumerLogger.csproj", "KafkaConsumerLogger/"]
RUN dotnet restore "KafkaConsumerLogger/KafkaConsumerLogger.csproj"

# Copy remaining source code and publish
COPY . .
WORKDIR /src/KafkaConsumerLogger
RUN dotnet publish "KafkaConsumerLogger.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app

RUN adduser --disabled-password --gecos "" appuser && chown -R appuser:appuser /app
USER appuser

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "KafkaConsumerLogger.dll"]
