FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["MultiTenantApp.Website/MultiTenantApp.Website.csproj", "MultiTenantApp.Website/"]
COPY ["MultiTenantApp.Persistance/MultiTenantApp.Persistance.csproj", "MultiTenantApp.Persistance/"]
COPY ["MultiTenantApp.Application/MultiTenantApp.Application.csproj", "MultiTenantApp.Application/"]
COPY ["MultiTenantApp.Domain/MultiTenantApp.Domain.csproj", "MultiTenantApp.Domain/"]
RUN dotnet restore "MultiTenantApp.Website/MultiTenantApp.Website.csproj"
COPY . .
WORKDIR "/src/MultiTenantApp.Website"
RUN dotnet build "MultiTenantApp.Website.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MultiTenantApp.Website.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MultiTenantApp.Website.dll"]