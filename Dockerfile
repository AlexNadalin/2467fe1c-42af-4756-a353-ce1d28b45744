# Build stage: restore and compile the solution
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy the project files on their own first, so the restore layer is cached until dependencies change
COPY CodeTest.slnx Directory.Build.props .editorconfig ./
COPY CodeTest/CodeTest.csproj CodeTest/
COPY CodeTest.Tests/CodeTest.Tests.csproj CodeTest.Tests/
RUN dotnet restore CodeTest.slnx

COPY . .
RUN dotnet build CodeTest.slnx --no-restore --configuration Release

# Test stage: the image can only be built if every unit test passes
FROM build AS test
RUN dotnet test CodeTest.slnx --no-build --configuration Release

# Publish stage: produce the console app's release output
FROM test AS publish
RUN dotnet publish CodeTest/CodeTest.csproj --no-build --configuration Release --output /app/publish

# Final stage: a small runtime-only image that runs the console app as a non-root user
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER $APP_UID
ENTRYPOINT ["dotnet", "CodeTest.dll"]
