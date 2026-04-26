using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using HaoKao.TenantService.Infrastructure;

namespace HaoKao.TenantService.WebApi.App_Data;

public class TenantDbContextFactory : IDesignTimeDbContextFactory<TenantDbContext>
{
    public TenantDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TenantDbContext>();
        optionsBuilder.UseMySql("Server=localhost,Port=3306;database=apiwuyf;User ID=root;Password=wuyf123456;Character Set=utf8;",
            new MySqlServerVersion(new Version(8, 0, 25)));
        return new TenantDbContext(optionsBuilder.Options);
    }
}