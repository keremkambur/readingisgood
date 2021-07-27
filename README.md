# readingisgood

Reading-Is-Good is a .NET 5 Web API project. It consists on multi-layer architecture and following clean code principles as much as possible.

Besides the actual Api layer, there are four more other projects present following separation of concern. 

*BusinessLayer* contains request/response models, services used by controllers/actions. Besides, various extension methods responsible for model transformation, HttpContext helpers and so on. *BusinessObject* is the main entry point when it comes to access *DataLayer*. Last but not least, it also keeps middlewares and exceptions.

*DataLayer* mostly keeps entity mappings, repositories and minor helper methods when it comes to transforming data between *BusinessLayer* and *EntityLayer*. Migrations are also managed by this layer.

*EntityLayer* keeps the models of actual database tables and some important enums.

*Utils* keeps JWT and other miscellaneous configurations.

Project is containerized and deployed as Azure web service on my personal subscription. API methods can be accessible via https://reading-is-good.azurewebsites.net/api/{controller}/{action} and Swagger is also configured and can be seen on https://reading-is-good.azurewebsites.net/swagger

Database preference is Azure Managed MSSQL. Web App authenticates against database via Azure Active Directory managed identity integration. EF Core 5 code-first approach followed. ConnectionString configuration on appsettings.json can be changed to desired connection. Migrations should work on any MSSQL instance.
