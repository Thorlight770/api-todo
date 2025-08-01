# Use the appropriate base image for your ASP.NET Core application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5001

# Set the URL for the ASP.NET Core application
ENV ASPNETCORE_URLS=http://+:5001

# Set the ASP.NET Core environment to 'Development'
ENV ASPNETCORE_ENVIRONMENT=Development

# Build and publish your ASP.NET Core application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ../nuget.config ./nuget.config
COPY ../local-package ./local-package
COPY . ./api.todo

WORKDIR /src/api.todo

RUN dotnet restore --configfile ./nuget.config

FROM build AS publish
RUN dotnet publish "api.todo/api.todo.csproj" -c Release -o /app/publish

# Finalize and run your ASP.NET Core application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "api.todo.dll"]
