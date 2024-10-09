# Use the SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the csproj and restore any dependencies (via `dotnet restore`) 
COPY ["Api/Api.csproj", "Api/"]
COPY ["BL/BL.csproj", "BL/"]
COPY ["DA/DA.csproj", "DA/"]
RUN dotnet restore "Api/Api.csproj"

# Copy the remaining files and build the project
COPY . .
WORKDIR "/src/Api"
RUN dotnet build "Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish

# Final stage/image use the official ASP.NET Core runtime as the base image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
EXPOSE 8080

