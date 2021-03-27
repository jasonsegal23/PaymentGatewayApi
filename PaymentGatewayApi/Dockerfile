#
# Stage 1 - Build .Net Core API
#

# Base image is alpine .NET SDK 
FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build-env

# Set the working directory
WORKDIR /app

# Copy the solution file to the container
COPY /PaymentGatewayApi.sln ./

# Copy the API and Test csproj files to the container
COPY /PaymentGatewayApi/PaymentGatewayApi.csproj ./PaymentGatewayApi/PaymentGatewayApi.csproj
COPY /PaymentGatewayApiTests/PaymentGatewayApiTests.csproj ./PaymentGatewayApiTests/PaymentGatewayApiTests.csproj

# Dotnet restore to install any dependencies
RUN dotnet restore PaymentGatewayApi.sln

# Copy all other files and build
COPY . ./
RUN dotnet publish -o out

#
# Stage 2 - Build .Net Core runtime
#

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine
WORKDIR /app
COPY --from=build-env /app/out .

COPY cert.pfx cert.pfx

RUN update-ca-certificates

EXPOSE 5001

ENTRYPOINT ["dotnet", "PaymentGatewayApi.dll"]