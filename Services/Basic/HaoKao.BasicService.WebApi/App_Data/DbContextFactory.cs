using System;
using HaoKao.BasicService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HaoKao.BasicService.WebApi.App_Data;
#if DEBUG

public class DbContextFactory : IDesignTimeDbContextFactory<BasicDbContext>
{
    public BasicDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BasicDbContext>();
        optionsBuilder.UseMySql(
            "Server=192.168.51.166;database=Wb_BasicManagementTest1;User ID=root;Password=123456;Character Set=utf8;",
            new MySqlServerVersion(new Version(8, 0, 25)));
        return new BasicDbContext(optionsBuilder.Options);
    }
}
#endif