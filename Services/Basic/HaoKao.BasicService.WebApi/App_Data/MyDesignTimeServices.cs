using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace HaoKao.BasicService.WebApi.App_Data;

public class MyDesignTimeServices : IDesignTimeServices
{
    public void ConfigureDesignTimeServices(IServiceCollection services)
    {
        //services.AddSingleton<IMigrationsCodeGenerator, GirvsCSharpMigrationsGenerator>();
        //services.AddSingleton<ICSharpMigrationOperationGenerator, GirvsCSharpMigrationOperationGenerator>();
    }
}