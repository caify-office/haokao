using HaoKao.QuestionService.Domain.QuestionModule;
using HaoKao.QuestionService.Domain.QuestionWrongModule;
using HaoKao.QuestionService.Domain.Works;
using Microsoft.Extensions.DependencyInjection;

namespace HaoKao.QuestionService.Infrastructure.Works;

public class CleanDuplicateQuestionWrongWork(IServiceProvider provider) : ICleanDuplicateQuestionWrongWork
{
    public async Task ExecuteAsync()
    {
        await using var scope = provider.CreateAsyncScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<QuestionDbContext>();
        var tables = await dbContext.GetTableNameList(nameof(QuestionWrong));
        foreach (var table in tables)
        {
            await using var tenantScope = provider.CreateTenantAsyncScope(table);
            await using var tenantDbContext = tenantScope.ServiceProvider.GetRequiredService<QuestionDbContext>();
            tenantDbContext.ShardingAutoMigration();

            var list = await tenantDbContext.QuestionWrongs.ToListAsync();

            foreach (var item in list.GroupBy(x => x.CreatorId))
            {
                var ids = item.GroupBy(x => x.QuestionId).Select(x => x.Max(y => y.Id)).ToList();
                var duplicate = item.Where(x => !ids.Contains(x.Id)).ToList();
                tenantDbContext.QuestionWrongs.RemoveRange(duplicate);
            }

            await tenantDbContext.SaveChangesAsync();
        }
    }
}

public class InitQuestionWrongSortWork(IServiceProvider provider) : IInitQuestionWrongSortWork
{
    public async Task ExecuteAsync()
    {
        await using var scope = provider.CreateAsyncScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<QuestionDbContext>();
        var tables = await dbContext.GetTableNameList(nameof(QuestionWrong));
        foreach (var table in tables)
        {
            await using var tenantScope = provider.CreateTenantAsyncScope(table);
            await using var tenantDbContext = tenantScope.ServiceProvider.GetRequiredService<QuestionDbContext>();
            tenantDbContext.ShardingAutoMigration();

            var list = await tenantDbContext.QuestionWrongs.ToListAsync();

            foreach (var item in list)
            {
                item.Sort = Array.IndexOf(QuestionType.All, item.QuestionTypeId);
            }

            await tenantDbContext.SaveChangesAsync();
        }
    }
}

public class FixQuestionWrongTypeIdWork(IServiceProvider provider) : IFixQuestionWrongTypeIdWork
{
    public async Task ExecuteAsync()
    {
        await using var scope = provider.CreateAsyncScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<QuestionDbContext>();
        var tables = await dbContext.GetTableNameList(nameof(QuestionWrong));
        foreach (var table in tables)
        {
            await using var tenantScope = provider.CreateTenantAsyncScope(table);
            await using var tenantDbContext = tenantScope.ServiceProvider.GetRequiredService<QuestionDbContext>();
            tenantDbContext.ShardingAutoMigration();

            var list = await tenantDbContext.QuestionWrongs.ToListAsync();
            var questions = await tenantDbContext.Questions.Select(x => new Question
            {
                Id = x.Id,
                QuestionTypeId = x.QuestionTypeId
            }).ToListAsync();

            foreach (var item in list)
            {
                item.QuestionTypeId = questions.FirstOrDefault(x => x.Id == item.QuestionId)?.QuestionTypeId ?? Guid.Empty;
                item.ParentQuestionTypeId = questions.FirstOrDefault(x => x.Id == item.QuestionId)?.QuestionTypeId ?? Guid.Empty;
                item.Sort = Array.IndexOf(QuestionType.All, item.QuestionTypeId);
            }

            await tenantDbContext.SaveChangesAsync();
        }
    }
}