
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY  TesteDigitas.sln ./
COPY  TesteDigitas.Api/TesteDigitas.Api.csproj ./TesteDigitas.Api/
COPY  TesteDigitas.Application/TesteDigitas.Application.csproj ./TesteDigitas.Application/
COPY  TesteDigitas.Domain/TesteDigitas.Domain.csproj ./TesteDigitas.Domain/
COPY  TesteDigitas.Infrastructure/TesteDigitas.Infrastructure.csproj ./TesteDigitas.Infrastructure/									   
COPY  XUnit.Coverlet.Collector/XUnit.Coverlet.Collector.csproj ./XUnit.Coverlet.Collector/
COPY  XUnit.Coverlet.MSBuild/XUnit.Coverlet.MSBuild.csproj ./XUnit.Coverlet.MSBuild/

RUN dotnet restore 

COPY . .

WORKDIR /src/TesteDigitas.Api
# Publish the application
RUN dotnet publish -c Release -o /app

WORKDIR /src/TesteDigitas.Application
# Publish the application
RUN dotnet publish -c Release -o /app

WORKDIR /src/TesteDigitas.Domain
# Publish the application
RUN dotnet publish -c Release -o /app

WORKDIR /src/TesteDigitas.Infrastructure
# Publish the application
RUN dotnet publish -c Release -o /app

WORKDIR /src/XUnit.Coverlet.Collector
# Publish the application
RUN dotnet publish -c Release -o /app

WORKDIR /src/XUnit.Coverlet.MSBuild
# Publish the application
RUN dotnet publish -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
#COPY --from=build /app/out ./TesteDigitas.Api/
COPY --from=publish /app .

# Expose the port your application will run on
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet", "TesteDigitas.Api.dll"]

