FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["PartyMaker.Application/PartyMaker.Application.csproj", "PartyMaker.Application/"]
COPY ["PartyMaker.Story/PartyMaker.Story.csproj", "PartyMaker.Story/"]
COPY ["PartyMaker.Domain/PartyMaker.Domain.csproj", "PartyMaker.Domain/"]
COPY ["PartyMaker.Common/PartyMaker.Common.csproj", "PartyMaker.Common/"]
COPY ["PartyMaker.Dto/PartyMaker.Dto.csproj", "PartyMaker.Dto/"]
COPY ["PartyMaker.Configuration/PartyMaker.Configuration.csproj", "PartyMaker.Configuration/"]
COPY ["PartyMaker.Schema/PartyMaker.Schema.csproj", "PartyMaker.Schema/"]
COPY ["PartyMaker.DataAccess/PartyMaker.DataAccess.csproj", "PartyMaker.DataAccess/"]
RUN dotnet restore "PartyMaker.Application/PartyMaker.Application.csproj"
COPY . .
WORKDIR "/src/PartyMaker.Application"
RUN dotnet build "PartyMaker.Application.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "PartyMaker.Application.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "НУ.dll"]