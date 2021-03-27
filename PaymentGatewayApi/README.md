# PaymentGatewayApi

## Useful Links

**Postman Collection** - https://app.getpostman.com/join-team?invite_code=21bd64f8dbec0bd0d0cfd954b0aace34&ws=ae4ea056-77c7-48c2-a9a2-dc4c2eac7d18

## Local Setup

### Running in Local

Pre-requisites:
* MongoDB
* MongoDB Compass
* .NET 5.0

Steps (.NET CLI):
* `cd PaymentGatewayApi`
* `dotnet run`

Steps (Visual Studio):
* Change launch profile to 'PaymentGatewayApi' (not IIS)
* Run the application

### Running in Docker

Pre-requisites:
* Docker-Desktop

Steps:
* `docker-compose up -d --build`
