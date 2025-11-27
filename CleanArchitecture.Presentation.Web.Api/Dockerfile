#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["CleanArchitecture.Presentation.Web.API/CleanArchitecture.Presentation.Web.API.csproj", "CleanArchitecture.Presentation.Web.API/"]
COPY ["CleanArchitecture.Core.Application/CleanArchitecture.Core.Application.csproj", "CleanArchitecture.Core.Application/"]
COPY ["CleanArchitecture.Core.Domain/CleanArchitecture.Core.Domain.csproj", "CleanArchitecture.Core.Domain/"]
COPY ["CleanArchitecture.Infrastructure.Persistence/CleanArchitecture.Infrastructure.Email.csproj", "CleanArchitecture.Infrastructure.Email/"]
COPY ["CleanArchitecture.Infrastructure.Persistence/CleanArchitecture.Infrastructure.Persistence.csproj", "CleanArchitecture.Infrastructure.Persistence/"]
COPY ["CleanArchitecture.Infrastructure.Shared/CleanArchitecture.Infrastructure.Shared.csproj", "CleanArchitecture.Infrastructure.Shared/"]
COPY ["CleanArchitecture.Infrastructure.IdentityProvider/CleanArchitecture.Infrastructure.IdentityProvider.csproj", "CleanArchitecture.Infrastructure.IdentityProvider/"]
RUN dotnet restore "CleanArchitecture.Presentation.Web.API/CleanArchitecture.Presentation.Web.API.csproj"
COPY . .
WORKDIR "/src/CleanArchitecture.Presentation.Web.API"
RUN dotnet build "CleanArchitecture.Presentation.Web.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CleanArchitecture.Presentation.Web.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanArchitecture.Presentation.Web.API.dll"]