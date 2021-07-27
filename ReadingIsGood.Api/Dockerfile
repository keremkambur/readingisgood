#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY ["ReadingIsGood.Api/ReadingIsGood.Api.csproj", "ReadingIsGood.Api/"]
COPY ["ReadingIsGood.BusinessLayer/ReadingIsGood.BusinessLayer.csproj", "ReadingIsGood.BusinessLayer/"]
COPY ["ReadingIsGood.DataLayer/ReadingIsGood.DataLayer.csproj", "ReadingIsGood.DataLayer/"]
COPY ["ReadingIsGood.EntityLayer/ReadingIsGood.EntityLayer.csproj", "ReadingIsGood.EntityLayer/"]
COPY ["ReadingIsGood.Utils/ReadingIsGood.Utils.csproj", "ReadingIsGood.Utils/"]

RUN dotnet restore "ReadingIsGood.Api/ReadingIsGood.Api.csproj"
COPY . .
WORKDIR "/src/ReadingIsGood.Api"
RUN dotnet build "ReadingIsGood.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ReadingIsGood.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReadingIsGood.Api.dll"]