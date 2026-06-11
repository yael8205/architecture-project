# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore only the dependencies
COPY ["LotteryApi/LotteryApi.csproj", "LotteryApi/"]
RUN dotnet restore "LotteryApi/LotteryApi.csproj"

# Copy remaining source code and publish
COPY . .
WORKDIR /src/LotteryApi
RUN dotnet publish "LotteryApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Create a non-root user for the runtime container
RUN adduser --disabled-password --gecos "" appuser && chown -R appuser:appuser /app
USER appuser

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "LotteryApi.dll"]
