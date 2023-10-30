# shop

Requirements

Donet 7
Docker
AWS CLIAzure Data Studio / SSMS

// add google secret
dotnet user-secrets init
dotnet user-secrets set "Authentication:Google:ClientId" “Your google client id”
dotnet user-secrets set "Authentication:Google:ClientSecret" “Your google secret”

update <UserSecretsId> in docker compose with user secrete from csproj

//Add was config
Create S3 container and update container name in constants
aws configure --profile "local-dev-profile"

Update <YourServerPassword> in the docker compose and appsettings

navigate to the docker-compose location in your terminal and run
docker-compose build
docker-compose up

To run the App from the IDE
Make sure the server is up and then change in the appsettings
Change Server=shop-server-1 to Server=localhost
