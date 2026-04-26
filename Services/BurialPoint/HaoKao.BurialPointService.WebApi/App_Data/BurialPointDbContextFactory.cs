using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using HaoKao.BurialPointService.Infrastructure;

namespace HaoKao.BurialPointService.WebApi.App_Data;

public class BurialPointDbContextFactory : IDesignTimeDbContextFactory<BurialPointDbContext>
{
    public BurialPointDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BurialPointDbContext>();
        optionsBuilder.UseMySql("Server=localhost,Port=3306;database=apiwuyf;User ID=root;Password=wuyf123456;Character Set=utf8;",
            new MySqlServerVersion(new Version(8, 0, 25)));
        return new BurialPointDbContext(optionsBuilder.Options);
    }
}