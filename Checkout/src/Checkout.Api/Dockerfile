# Build context: the Checkout/ folder (see docker-compose.yml at the repo root).
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY src/ src/
RUN dotnet publish src/Checkout.Api/Checkout.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
# SQLite file lives in /data so it can be mounted as a volume.
RUN mkdir -p /data && chown $APP_UID /data
ENV ASPNETCORE_HTTP_PORTS=8080
ENV ConnectionStrings__Default="Data Source=/data/checkout.db"
USER $APP_UID
EXPOSE 8080
ENTRYPOINT ["dotnet", "Checkout.Api.dll"]
