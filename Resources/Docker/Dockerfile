#####################################################
#                      Prepare                      #
#####################################################
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine3.14 as final

# Database connection vars
ARG DATABASE_HOST
ARG DATABASE_PORT
ARG DATABASE_NAME
ARG DATABASE_USERNAME
ARG DATABASE_PASSWORD
ENV SSO_DB_CONNECTION_STRING="Server=$DATABASE_HOST; Port=$DATABASE_PORT; Database=$DATABASE_NAME; Username=$DATABASE_USERNAME; password=$DATABASE_PASSWORD;"

# ASPnet vars
ARG CERTIFICATE_PASSWORD
ARG ASPNETCORE_ENVIRONMENT
ENV ASPNETCORE_ENVIRONMENT=$ASPNETCORE_ENVIRONMENT
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/app/CERTS/TCD_Certificate.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=$CERTIFICATE_PASSWORD

# RabbitMQ vars
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

WORKDIR /app
RUN mkdir CERTS
COPY Resources/CERTS/TCD_Certificate.pfx ./CERTS/TCD_Certificate.pfx
EXPOSE 5005 5004

# ------------------------------------------------- #

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine3.14 as build-test
WORKDIR /usr/src/app
COPY ./ ./


#####################################################
#                      Restore                      #
#####################################################
FROM build-test as prepare
ARG NUGET_REPO_URL
ARG NUGET_REPO_USER
ARG NUGET_REPO_PASS

RUN dotnet nuget add source "$NUGET_REPO_URL/nuget/tcd-nuget/" \
            --name Artifactory \
            --username "$NUGET_REPO_USER" \
            --password "$NUGET_REPO_PASS" \
            --store-password-in-clear-text

RUN sleep 15 && dotnet nuget list source

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
ENTRYPOINT dotnet SingleSignOn.dll
