#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic AS base
ENV DEBIAN_FRONTEND=noninteractive
# RUN apt update && apt install tzdata -y
ENV TZ=Asia/Ho_Chi_Minh
# RUN apk update && \
#     apk add --no-cache tzdata
# RUN apt-get install -y tzdata
RUN apt-get update && \
    apt-get install -yq tzdata && \
    ln -fs /usr/share/zoneinfo/${TZ} /etc/localtime && \
    dpkg-reconfigure -f noninteractive tzdata
# RUN apt-get add tzdata \
#         && cp /usr/share/zoneinfo/${TZ} /etc/localtime \
#         && echo "${TZ}" > /etc/timezone
# RUN apt-get update
# RUN apt-get install -y software-properties-common
# RUN add-apt-repository ppa:deadsnakes/ppa
# RUN add-apt-repository ppa:libreoffice/libreoffice-7-0
# RUN apt-get update
# RUN apt-get install -y libreoffice python3-pip tzdata
# RUN apt-get clean
# RUN pip3 install --upgrade pip
# RUN pip3 install PyMuPDF==1.18.10

# ENV TZ Asia/Ho_Chi_Minh

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-bionic AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["./APIProject.sln", ""]
COPY ["./APIProject.Domain/APIProject.Domain.csproj", "./APIProject.Domain/"]
COPY ["./APIProject.Service/APIProject.Service.csproj", "./APIProject.Service/"]
COPY ["./APIProject.Repository/APIProject.Repository.csproj", "./APIProject.Repository/"]
COPY ["./APIProject.Common/APIProject.Common.csproj", "./APIProject.Common/"]
COPY ["./APIProject/APIProject.csproj", "./APIProject/"]


RUN dotnet restore
#
# Copy everything else and build
COPY . .
WORKDIR "/src/."
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# COPY ["./APIProject/APIProject.xml", "./APIProject/"]
COPY ["./APIProject/APIProject.xml", ""]

ENTRYPOINT ["dotnet", "APIProject.dll"]
