FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine as build
WORKDIR /app
COPY . .
#RUN dotnet restore
#RUN dotnet publish -o /app/published-app --configuration Debug

# copy csproj and restore as distinct layers
#COPY *.sln .
#COPY bramvandenbussche.readwiser.api/*.csproj ./bramvandenbussche.readwiser.api/
#COPY bramvandenbussche.readwiser.api.tests/*.csproj ./bramvandenbussche.readwiser.api.tests/
#COPY bramvandenbussche.readwiser.domain/*.csproj ./bramvandenbussche.readwiser.domain/
#COPY bramvandenbussche.readwiser.web/*.csproj ./bramvandenbussche.readwiser.web/
#COPY bramvandenbussche.readwiser.data.mongodb/*.csproj ./bramvandenbussche.readwiser.data.mongodb/

RUN dotnet restore

# copy everything else and build app
#COPY bramvandenbussche.readwiser.api/. ./bramvandenbussche.readwiser.api/
#COPY bramvandenbussche.readwiser.domain/. /bramvandenbussche.readwiser.domain/
#COPY bramvandenbussche.readwiser.data.mongodb/. ./bramvandenbussche.readwiser.data.mongodb/
#
WORKDIR /app/bramvandenbussche.readwiser.web
RUN dotnet publish -c Release -o /app/published-app

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine as runtime
WORKDIR /app
COPY --from=build /app/published-app /app
ENTRYPOINT [ "dotnet", "/app/bramvandenbussche.readwiser.web.dll" ]

EXPOSE 80