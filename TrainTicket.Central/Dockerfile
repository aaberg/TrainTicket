FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TrainTicket.Central/TrainTicket.Central.csproj", "TrainTicket.Central/"]
RUN dotnet restore "TrainTicket.Central/TrainTicket.Central.csproj"
COPY . .
WORKDIR "/src/TrainTicket.Central"
RUN dotnet build "TrainTicket.Central.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TrainTicket.Central.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TrainTicket.Central.dll"]
