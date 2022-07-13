#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["TrustBank.API/TrustBank.API.csproj", "TrustBank.API/"]
RUN dotnet restore "TrustBank.API/TrustBank.API.csproj"
COPY . .
WORKDIR "/src/TrustBank.API"
RUN dotnet build "TrustBank.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TrustBank.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TrustBank.API.dll"]