#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/ExPaper.Auth.API/ExPaper.Auth.API.csproj", "Services/ExPaper.Auth.API/"]
COPY ["Shared/ExPaper.SharedMethods.Lib/ExPaper.SharedMethods.Lib.csproj", "Shared/ExPaper.SharedMethods.Lib/"]
COPY ["Shared/ExPaper.SharedModels.Lib/ExPaper.SharedModels.Lib.csproj", "Shared/ExPaper.SharedModels.Lib/"]
RUN dotnet restore "Services/ExPaper.Auth.API/ExPaper.Auth.API.csproj"
COPY . .
WORKDIR "/src/Services/ExPaper.Auth.API"
RUN dotnet build "ExPaper.Auth.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExPaper.Auth.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExPaper.Auth.API.dll"]