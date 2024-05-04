# Use the appropriate base image for your ASP.NET Core application
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000

# Set the URL for the ASP.NET Core application
ENV ASPNETCORE_URLS=http://*:5000

# Set the ASP.NET Core environment to 'Development'
ENV ASPNETCORE_ENVIRONMENT=Development

# Build and publish your ASP.NET Core application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["api.todo/api.todo.csproj", "api.todo/"]
RUN dotnet restore "api.todo/api.todo.csproj"
COPY . .
RUN dotnet build "api.todo/api.todo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "api.todo/api.todo.csproj" -c Release -o /app/publish

# Finalize and run your ASP.NET Core application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "api.todo.dll"]
