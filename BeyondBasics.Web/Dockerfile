FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY BeyondBasics.Web.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# aspnet env port
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000


ENTRYPOINT ["dotnet", "BeyondBasics.Web.dll"]
