# Use the official .NET Core SDK as a parent image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app
ENV DOTNET_URLS=http://*:5000
EXPOSE 5000

# Copy the project file and restore any dependencies (use .csproj for the project name)
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

ENV DOTNET_ENVIRONMENT=Development
WORKDIR /src
COPY ["api.todo/api.todo.csproj", "api.todo/"]
RUN dotnet restore "api.todo/api.todo.csproj"
COPY . .
RUN dotnet build "api.todo/api.todo.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "api.todo/api.todo.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Build the runtime image
FROM base AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "api.todo.dll"]