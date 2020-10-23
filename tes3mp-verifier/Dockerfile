#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["tes3mp-verifier/tes3mp-verifier.csproj", "tes3mp-verifier/"]
RUN dotnet restore "tes3mp-verifier/tes3mp-verifier.csproj"
COPY . .
WORKDIR "/src/tes3mp-verifier"
RUN dotnet build "tes3mp-verifier.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "tes3mp-verifier.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "tes3mp-verifier.dll"]