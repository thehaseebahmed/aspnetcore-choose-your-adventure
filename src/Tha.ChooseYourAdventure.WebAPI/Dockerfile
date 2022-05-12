#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Tha.ChooseYourAdventure.WebAPI/Tha.ChooseYourAdventure.WebAPI.csproj", "Tha.ChooseYourAdventure.WebAPI/"]
COPY ["Tha.ChooseYourAdventure.Library/Tha.ChooseYourAdventure.Library.csproj", "Tha.ChooseYourAdventure.Library/"]
COPY ["Tha.ChooseYourAdventure.Models/Tha.ChooseYourAdventure.Data.csproj", "Tha.ChooseYourAdventure.Models/"]
RUN dotnet restore "Tha.ChooseYourAdventure.WebAPI/Tha.ChooseYourAdventure.WebAPI.csproj"
COPY . .
WORKDIR "/src/Tha.ChooseYourAdventure.WebAPI"
RUN dotnet build "Tha.ChooseYourAdventure.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tha.ChooseYourAdventure.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tha.ChooseYourAdventure.WebAPI.dll"]