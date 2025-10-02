# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY EmployeImportCSVCrud/*.csproj ./EmployeImportCSVCrud/
RUN dotnet restore EmployeImportCSVCrud/EmployeImportCSVCrud.csproj

COPY EmployeImportCSVCrud/ ./EmployeImportCSVCrud/
WORKDIR /src/EmployeImportCSVCrud
RUN dotnet publish -c Release -o /app

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "EmployeImportCSVCrud.dll"]
