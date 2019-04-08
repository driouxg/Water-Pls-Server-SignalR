# First stage of multi-stage build
#FROM microsoft/aspnetcore-build AS build-env
FROM microsoft/dotnet:2.2-sdk
WORKDIR /app

# copy the contents of agent working directory on host to workdir in container
#COPY . ./
COPY *.csproj ./
RUN dotnet restore

# dotnet commands to build, test, and publish
# RUN dotnet restore
# RUN dotnet build -c Release
# RUN dotnet publish -c Release -o out

# Second stage - Build runtime image
#FROM microsoft/aspnetcore
#WORKDIR /app
#COPY --from=build-env /app/out .
COPY . ./
RUN dotnet publish -c Release -o out
ENTRYPOINT ["dotnet", "out/water-pls-server.dll"]