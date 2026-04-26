using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using HaoKao.LecturerService.Infrastructure;

namespace HaoKao.LecturerService.WebApi.App_Data;

public class LecturerDbContextFactory : IDesignTimeDbContextFactory<LecturerDbContext>
{
    public LecturerDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<LecturerDbContext>();
        optionsBuilder.UseMySql("Server=localhost,Port=3306;database=apiwuyf;User ID=root;Password=wuyf123456;Character Set=utf8;",
            new MySqlServerVersion(new Version(8, 0, 25)));
        return new LecturerDbContext(optionsBuilder.Options);
    }
}