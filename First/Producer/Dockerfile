#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["First/Producer/Producer.csproj", "First/Producer/"]
COPY ["Core/Core.Infrastracture.RabbitMQ/Core.Infrastracture.RabbitMQ.csproj", "Core/Core.Infrastracture.RabbitMQ/"]
COPY ["First/Shared/Shared.csproj", "First/Shared/"]
RUN dotnet restore "First/Producer/Producer.csproj"
COPY . .
WORKDIR "/src/First/Producer"
RUN dotnet build "Producer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Producer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Producer.dll"]
