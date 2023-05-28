FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ImageService.csproj .
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app .
RUN mkdir Images
ENTRYPOINT ["dotnet", "ImageService.dll"]