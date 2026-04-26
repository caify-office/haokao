using Girvs.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations.Design;

namespace ShortUrlService.WebApi.App_Data;

public class MyDesignTimeServices : IDesignTimeServices
{
    public void ConfigureDesignTimeServices(IServiceCollection services)
    {
        services.AddSingleton<IMigrationsCodeGenerator, GirvsCSharpMigrationsGenerator>();
        services.AddSingleton<ICSharpMigrationOperationGenerator, GirvsCSharpMigrationOperationGenerator>();
    }
}