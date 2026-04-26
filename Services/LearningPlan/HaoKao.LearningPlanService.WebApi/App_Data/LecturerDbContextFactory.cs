using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using HaoKao.LearningPlanService.Infrastructure;

namespace HaoKao.LearningPlanService.WebApi.App_Data;

public class LearningPlanDbContextFactory : IDesignTimeDbContextFactory<LearningPlanDbContext>
{
    public LearningPlanDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<LearningPlanDbContext>();
        optionsBuilder.UseMySql("Server=localhost,Port=3306;database=apiwuyf;User ID=root;Password=wuyf123456;Character Set=utf8;",
            new MySqlServerVersion(new Version(8, 0, 25)));
        return new LearningPlanDbContext(optionsBuilder.Options);
    }
}