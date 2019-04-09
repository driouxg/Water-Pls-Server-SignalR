# First stage of multi-stage build
# Example from: https://docs.docker.com/engine/examples/dotnetcore/

# Dockerfile cheat sheet: https://kapeli.com/cheat_sheets/Dockerfile.docset/Contents/Resources/Documents/index
FROM microsoft/dotnet:2.2-sdk AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY SignalRTest/*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
# FROM microsoft/dotnet:aspnetcore-runtime
# WORKDIR /app
# COPY --from=build-env /out
ENTRYPOINT ["dotnet", "out/SignalRTest.dll"]