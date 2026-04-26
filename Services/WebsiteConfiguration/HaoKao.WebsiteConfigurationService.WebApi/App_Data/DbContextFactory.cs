using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using HaoKao.WebsiteConfigurationService.Infrastructure;

namespace HaoKao.WebsiteConfigurationService.WebApi.App_Data;

public class DbContextFactory : IDesignTimeDbContextFactory<WebsiteConfigurationDbContext>
{
    public WebsiteConfigurationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WebsiteConfigurationDbContext>();
        optionsBuilder.UseMySql("Server=localhost,Port=3306;database=apiwuyf;User ID=root;Password=wuyf123456;Character Set=utf8;",
            new MySqlServerVersion(new Version(8, 0, 25)));
        return new WebsiteConfigurationDbContext(optionsBuilder.Options);
    }
}