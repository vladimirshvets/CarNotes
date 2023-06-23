#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Presentation/CarNotes.WebAPI/CarNotes.WebAPI.csproj", "src/Presentation/CarNotes.WebAPI/"]
COPY ["src/Core/CarNotes.Application/CarNotes.Application.csproj", "src/Core/CarNotes.Application/"]
COPY ["src/Core/CarNotes.Domain/CarNotes.Domain.csproj", "src/Core/CarNotes.Domain/"]
COPY ["src/Infrastructure/CarNotes.Persistence.Neo4j/CarNotes.Persistence.Neo4j.csproj", "src/Infrastructure/CarNotes.Persistence.Neo4j/"]
RUN dotnet restore "src/Presentation/CarNotes.WebAPI/CarNotes.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/Presentation/CarNotes.WebAPI"
RUN dotnet build "CarNotes.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarNotes.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarNotes.WebAPI.dll"]
