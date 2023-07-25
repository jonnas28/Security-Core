# Security-Core
Web security 

Database MYSQL
- Open docker
- copy docker-compose and paste it in the path whatever you want. 
- open cmd on the folder where the docker-compose.yml file location.
- run "docker compose up"


*requirements
 to install dotnet cli run "dotnet tool install --global dotnet-ef"

Migrate to database
- Type "dotnet ef migrations add AddMiddleNameUser --context "ApplicationContext" --project "E:\Personal Project\Security\Security-Core\Identity\Identity.csproj" --startup-project "E:\Personal Project\Security\Security-Core\WebAPI\WebAPI.csproj" "

Update Database
- Type " dotnet ef database update --startup-project ".\WebAPI\WebAPI.csproj" --connection "Server=localhost; Database=Security; User Id=root; Password=P@ssw0rd;" --context "ApplicationContext""




    