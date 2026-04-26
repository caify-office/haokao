using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using HaoKao.DrawPrizeService.Infrastructure;

namespace HaoKao.DrawPrizeService.WebApi.App_Data;

public class DbContextFactory : IDesignTimeDbContextFactory<DrawPrizeDbContext>
{
    public DrawPrizeDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DrawPrizeDbContext>();
        optionsBuilder.UseMySql("Server=localhost,Port=3306;database=apiwuyf;User ID=root;Password=wuyf123456;Character Set=utf8;",
            new MySqlServerVersion(new Version(8, 0, 25)));
        return new DrawPrizeDbContext(optionsBuilder.Options);
    }
}