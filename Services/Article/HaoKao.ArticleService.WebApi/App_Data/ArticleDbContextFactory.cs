using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using HaoKao.ArticleService.Infrastructure;

namespace HaoKao.ArticleService.WebApi.App_Data;

public class DbContextFactory : IDesignTimeDbContextFactory<ArticleDbContext>
{
    public ArticleDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ArticleDbContext>();
        optionsBuilder.UseMySql("Server=localhost,Port=3306;database=apiwuyf;User ID=root;Password=wuyf123456;Character Set=utf8;",
            new MySqlServerVersion(new Version(8, 0, 25)));
        return new ArticleDbContext(optionsBuilder.Options);
    }
}