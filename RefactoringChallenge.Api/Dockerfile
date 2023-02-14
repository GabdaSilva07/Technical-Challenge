FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# To Build the image in Azure DevOps pipeline


#Set variables in Azure DevOps pipeline to use in the Dockerfile
# Azure email Address
ARG AZURE_FEED_USERNAME

# Artifact Feed URL
ARG AZURE_ARTIFACT_FEED_URL

# Personal Access Token from Azure DevOps
ARG PAT_TOKEN

# To use Nuget packages from Azure Artifacts, instead of referencing the packages directly in the csproj file
ENV Package=1

# Download and install the Azure Artifacts Credential Provider. Not Required after https://github.com/dotnet/dotnet-docker/issues/878 as it is included in the SDK image
RUN curl -L https://aka.ms/install-artifacts-credprovider.sh | bash

#Optional: Sometimes the http client is not able to connect to the Azure Artifacts feed. This is a workaround to use sockets instead of http client
ENV DOTNET_SYSTEM_NET_HTTP_USESOCKETSHTTPHANDLER=0 

# Environment variables to enable session token caching
ENV NUGET_CREDENTIALPROVIDER_SESSIONTOKENCACHE_ENABLED=true

# Environment variables for adding endppoint credentials to the NuGet.config file
#Add "FEED_URL" and "PAT_TOKEN" using --build-arg in the docker build command
ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS="{\"endpointCredentials\": [{\"endpoint\":\"$AZURE_FEED_URL\",\"username\":\"$AZURE_FEED_USERNAME\",\"password\":\"$PAT_TOKEN\"}]}"


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "RefactoringChallenge.csproj"
RUN dotnet build "RefactoringChallenge.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RefactoringChallenge.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RefactoringChallenge.dll"]
