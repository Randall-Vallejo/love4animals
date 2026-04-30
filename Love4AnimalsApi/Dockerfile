FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Love4AnimalsApi.csproj", "./"]
RUN dotnet restore
COPY . .
RUN dotnet publish "Love4AnimalsApi.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Love4AnimalsApi.dll"]