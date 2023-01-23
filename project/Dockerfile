#FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
#WORKDIR /app
#EXPOSE 80
#RUN apt-get update --allow-releaseinfo-change
#RUN apt-get install ca-certificates -y
#COPY "GNBRCA2-SHA2-Root-CA.crt" "/usr/local/share/ca-certificates/"
#RUN update-ca-certificates

#FROM ubuntu:22.04 AS base
#WORKDIR /app
##EXPOSE 443
# Install "software-properties-common" (for the "add-apt-repository")
#RUN apt-get update && apt-get install -y \
#    software-properties-common

#Run wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
 #    dpkg -i packages-microsoft-prod.deb && \
  #   rm packages-microsoft-prod.deb

#Run apt-get update; \
     #apt-get install -y apt-transport-https && \
     #apt-get update && \
     #apt-get install -y dotnet-sdk-7.0

#Run apt-get update; \
#   apt-get install -y apt-transport-https && \
#    apt-get update && \
#    apt-get install -y aspnetcore-runtime-7.0
#RUN apt-get update --allow-releaseinfo-change

#RUN apt-get install iputils-ping
#RUN ping google.com

#RUN echo "DONE!"

#RUN apt-get install ca-certificates -y
#COPY "GNBRCA2-SHA2-Root-CA.crt" "/usr/local/share/ca-certificates/"
#RUN update-ca-certificates

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY "project.csproj" ./
RUN dotnet restore "project.csproj"

COPY . ./
RUN dotnet publish -c Release -o out

#WORKDIR /app
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Program.dll"]