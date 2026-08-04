# ------------------------------------------------------------
# Stage 1: Build, test and publish the application
# ------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

# Set the working directory inside the build container.
WORKDIR /source


# Copy project and solution metadata first.
# Keeping this separate from the source-code copy allows
# Docker to reuse the NuGet restore layer when source code
# changes but project dependencies do not.

COPY DotNetWebApp.slnx ./

COPY src/DotNetWebApp.Api/DotNetWebApp.Api.csproj \
     src/DotNetWebApp.Api/

COPY tests/DotNetWebApp.Api.Tests/DotNetWebApp.Api.Tests.csproj \
     tests/DotNetWebApp.Api.Tests/


# Restore the NuGet packages required by the test project.
RUN dotnet restore DotNetWebApp.slnx


# Copy the remaining application and test source code.
COPY src/ src/
COPY tests/ tests/

# Build the application in Release configuration.
RUN dotnet build \
    src/DotNetWebApp.Api/DotNetWebApp.Api.csproj \
    --configuration Release \
    --no-restore

# Run the unit tests and save the test results as a TRX file.
RUN dotnet test \
    tests/DotNetWebApp.Api.Tests/DotNetWebApp.Api.Tests.csproj \
    --configuration Release \
    --no-restore \
    --logger "trx" \
    --results-directory /source/artifacts/test-results

# Publish the application files required by the runtime container.
RUN dotnet publish \
    src/DotNetWebApp.Api/DotNetWebApp.Api.csproj \
    --configuration Release \
    --no-restore \
    --output /source/artifacts/publish


# ------------------------------------------------------------
# Stage 2: Create the final runtime image
# ------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

# Copy only the published application from the build stage.
COPY --from=build /source/artifacts/publish/ ./

# Document the default port used by the ASP.NET Core application.
EXPOSE 8080

# Start the web application when the container starts.
ENTRYPOINT ["dotnet", "DotNetWebApp.Api.dll"]