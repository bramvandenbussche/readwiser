FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine as build
WORKDIR /app

COPY . .
RUN dotnet restore

WORKDIR /app/bramvandenbussche.readwiser.web
RUN dotnet publish -c Release -o /app/published-app

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine as runtime
RUN apk add --no-cache icu-libs
WORKDIR /app
COPY --from=build /app/published-app /app
ENTRYPOINT [ "dotnet", "/app/bramvandenbussche.readwiser.web.dll" ]

EXPOSE 80