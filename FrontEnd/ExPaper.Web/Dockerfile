#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.


FROM arm64v8/ubuntu:latest AS base
#FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
#EXPOSE 443

RUN apt-get update && apt-get install -y \
    apt-transport-https \
    aspnetcore-runtime-7.0

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["FrontEnd/ExPaper.Web/ExPaper.Web.csproj", "FrontEnd/ExPaper.Web/"]

COPY ["Shared/ExPaper.SharedMethods.Lib/ExPaper.SharedMethods.Lib.csproj", "Shared/ExPaper.SharedMethods.Lib/"]
COPY ["Shared/ExPaper.SharedModels.Lib/ExPaper.SharedModels.Lib.csproj", "Shared/ExPaper.SharedModels.Lib/"]

RUN dotnet restore "FrontEnd/ExPaper.Web/ExPaper.Web.csproj"
COPY . .
WORKDIR "/src/FrontEnd/ExPaper.Web"
RUN dotnet build "ExPaper.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExPaper.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "ExPaper.Web.dll"]
ENTRYPOINT ["dotnet", "ExPaper.Web.dll", "-url http://192.168.175.15:7110"]