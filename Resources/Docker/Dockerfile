#####################################################
#                      Restore                      #
#####################################################
FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine3.13 as package-restore
WORKDIR /usr/src/app
COPY ./ ./

ARG NUGET_REPO_URL
ARG NUGET_REPO_USER
ARG NUGET_REPO_PASS

RUN dotnet nuget add source $NUGET_REPO_URL/nuget/tcd-nuget/ \
            --name Artifactory \
            --username $NUGET_REPO_USER \
            --password "$NUGET_REPO_PASS" \
            --store-password-in-clear-text

RUN dotnet nuget list source && sleep 15
RUN dotnet restore Source/SingleSignOn/SingleSignOn.sln
#RUN dotnet tool install dotnet-sonarscanner --global
#ENV PATH="${PATH}:/root/.dotnet/tools"

#####################################################
#                  Build & Test                     #
#####################################################
FROM package-restore AS build-test
#RUN apk add --no-cache \
#    openjdk11 \
#    nodejs


#ARG SonarProjectKey
#ARG SonarHost
#ARG SonarLogin
#RUN dotnet sonarscanner begin \
#    /k:$SonarProjectKey \
#    /d:sonar.host.url=$SonarHost \
#    /d:sonar.login=$SonarLogin \
#    /d:sonar.cs.opencover.reportsPaths=".\coverage.opencover.xml" \
#    /d:sonar.cs.vstest.reportsPaths=".\TestResults\*.trx" \
#    /n:$SonarProjectKey

RUN dotnet build -c Release --no-restore Source/SingleSignOn/SingleSignOn.sln
#RUN dotnet test \
#    /p:CollectCoverage=true \
#    /p:CoverletOutputFormat=opencover \
#    --collect:"Code Coverage" \
#    --logger trx \
#    --results-directory "TestResults"
#RUN dotnet sonarscanner end /d:sonar.login=$SonarLogin

#####################################################
#                     Publish                       #
#####################################################
FROM build-test AS published
RUN dotnet publish Source/SingleSignOn/SingleSignOn.sln -c Release -o /app/dotnet/

######################################################
#                       RUN                         #
######################################################
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine3.13

WORKDIR /app
COPY --from=published /app/dotnet/ .
EXPOSE 80 5001 5000
CMD [ "dotnet", "SingleSignOn.dll" ]
