#####################################################
#                      Prepare                      #
#####################################################
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine3.13 as final

WORKDIR /app
RUN mkdir CERTS
COPY Resources/CERTS/TCD_Certificate.pfx ./CERTS/TCD_Certificate.pfx
COPY Resources/Docker/start.sh ./start.sh
EXPOSE 5005 5004

# Database
ARG CERTIFICATE_PASSWORD
ARG DATABASE_HOST
ARG DATABASE_PORT
ARG DATABASE_NAME
ARG DATABASE_USERNAME
ARG DATABASE_PASSWORD
ENV SSO_DB_CONNECTION_STRING="Server=$DATABASE_HOST; Port=$DATABASE_PORT; Database=$DATABASE_NAME; Username=$DATABASE_USERNAME; password=$DATABASE_PASSWORD;"
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/app/CERTS/TCD_Certificate.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=$CERTIFICATE_PASSWORD

# RabbitMQ
ARG CORE_RABBITMQ_HOSTNAME
ARG CORE_RABBITMQ_PORT
ARG CORE_RABBITMQ_USERNAME
ARG CORE_RABBITMQ_PASSWORD
ARG CORE_RABBITMQ_VIRTUALHOST
ENV CORE_RABBITMQ_HOSTNAME=$CORE_RABBITMQ_HOSTNAME
ENV CORE_RABBITMQ_PORT=$CORE_RABBITMQ_PORT
ENV CORE_RABBITMQ_USERNAME=$CORE_RABBITMQ_USERNAME
ENV CORE_RABBITMQ_PASSWORD=$CORE_RABBITMQ_PASSWORD
ENV CORE_RABBITMQ_VIRTUALHOST=$CORE_RABBITMQ_VIRTUALHOST


FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine3.13 as build-test
WORKDIR /usr/src/app
COPY ./ ./
#####################################################
#                      Restore                      #
#####################################################
FROM build-test as prepare
ARG NUGET_REPO_URL
ARG NUGET_REPO_USER
ARG NUGET_REPO_PASS

RUN dotnet nuget add source $NUGET_REPO_URL/nuget/tcd-nuget/ \
            --name Artifactory \
            --username $NUGET_REPO_USER \
            --password "$NUGET_REPO_PASS" \
            --store-password-in-clear-text

# RUN dotnet nuget list source && sleep 15
RUN dotnet restore Source/SingleSignOn/SingleSignOn.sln

#####################################################
#                  Build & Test                     #
#####################################################
FROM prepare AS build-solution
RUN dotnet build -c Release --no-restore Source/SingleSignOn/SingleSignOn.sln

#####################################################
#                     Publish                       #
#####################################################
FROM build-solution AS published
RUN dotnet publish Source/SingleSignOn/SingleSignOn.sln --no-build -c Release -o /app/dotnet/

######################################################
#                       RUN                         #
######################################################
FROM final
COPY --from=published /app/dotnet/ .
ENTRYPOINT sh start.sh
