# Fonte: https://docs.microsoft.com/pt-br/aspnet/core/host-and-deploy/docker/building-net-docker-images?view=aspnetcore-3.0

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

COPY ./Resiliente.ServicoA.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

ARG port
ENV ASPNETCORE_URLS="http://*:$port"

ENTRYPOINT ["dotnet", "Resiliente.ServicoA.dll"]