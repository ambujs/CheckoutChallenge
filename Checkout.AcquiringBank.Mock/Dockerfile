#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
COPY ./Checkout.AcquiringBank.Mock.csproj /Checkout.AcquiringBank.Mock/
RUN dotnet restore ./Checkout.AcquiringBank.Mock/Checkout.AcquiringBank.Mock.csproj
COPY . ./Checkout.AcquiringBank.Mock/
WORKDIR /Checkout.AcquiringBank.Mock/
RUN dotnet build "Checkout.AcquiringBank.Mock.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Checkout.AcquiringBank.Mock.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Checkout.AcquiringBank.Mock.dll"]
