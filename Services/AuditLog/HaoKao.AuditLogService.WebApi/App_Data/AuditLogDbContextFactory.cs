using System;
using HaoKao.AuditLogService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HaoKao.AuditLogService.WebApi.App_Data;
#if DEBUG

public class AuditLogDbContextFactory : IDesignTimeDbContextFactory<AuditLogDbContext>
{
    public AuditLogDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AuditLogDbContext>();
        optionsBuilder.UseMySql("Server=192.168.51.166;database=Wb_AuditLog;User ID=root;Password=123456;Character Set=utf8;",
                                new MySqlServerVersion(new Version(8, 0, 25)));
        return new AuditLogDbContext(optionsBuilder.Options);
    }
}

#endif