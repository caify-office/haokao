using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using HaoKao.LiveBroadcastService.Infrastructure;

namespace HaoKao.LiveBroadcastService.WebApi.App_Data;

public class DbContextFactory : IDesignTimeDbContextFactory<LiveBroadcastDbContext>
{
    public LiveBroadcastDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<LiveBroadcastDbContext>();
        optionsBuilder.UseMySql("Server=localhost,Port=3306;database=apiwuyf;User ID=root;Password=wuyf123456;Character Set=utf8;",
                                new MySqlServerVersion(new Version(8, 0, 25)));
        return new LiveBroadcastDbContext(optionsBuilder.Options);
    }
}