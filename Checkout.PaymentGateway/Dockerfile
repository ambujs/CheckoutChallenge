#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
COPY ./Checkout.PaymentGateway.csproj /Checkout.PaymentGateway/
RUN dotnet restore ./Checkout.PaymentGateway/Checkout.PaymentGateway.csproj
COPY . ./Checkout.PaymentGateway/
WORKDIR /Checkout.PaymentGateway/
RUN dotnet build "Checkout.PaymentGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Checkout.PaymentGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Checkout.PaymentGateway.dll"]
