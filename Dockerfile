FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

ARG BUILDCONFIG=Release
ARG VERSION=1.0.0

WORKDIR /src
COPY ./src .
RUN dotnet publish PaymentGateway.Api/PaymentGateway.Api.csproj -c $BUILDCONFIG -o /app /p:Version=$VERSION

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app

COPY --from=build /app .

ENTRYPOINT ["dotnet", "PaymentGateway.Api.dll"]