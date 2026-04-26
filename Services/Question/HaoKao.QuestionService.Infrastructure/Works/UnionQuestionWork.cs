using HaoKao.QuestionService.Domain.QuestionModule;
using HaoKao.QuestionService.Domain.Works;
using Microsoft.Extensions.DependencyInjection;

namespace HaoKao.QuestionService.Infrastructure.Works;

public class UnionQuestionWork(IServiceProvider provider) : IUnionQuestionWork
{
    public async Task ExecuteAsync()
    {
        await using var scope = provider.CreateAsyncScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<QuestionDbContext>();
        var tables = await dbContext.GetTableNameList(nameof(Question));
        foreach (var table in tables)
        {
            await using var tenantScope = provider.CreateTenantAsyncScope(table);
            await using var tenantDbContext = tenantScope.ServiceProvider.GetRequiredService<QuestionDbContext>();
            tenantDbContext.ShardingAutoMigration();

            // add
            tenantDbContext.UnionQuestions.AddRange(
                tenantDbContext.Questions.AsNoTracking()
                .Where(x => !tenantDbContext.UnionQuestions.Any(y => y.Id == x.Id))
                .Select(x => new UnionQuestion
                {
                    Id = x.Id,
                    SubjectId = x.SubjectId,
                    ChapterNodeId = x.ChapterId,
                    QuestionCategoryId = x.QuestionCategoryId,
                    QuestionTypeId = x.QuestionTypeId,
                    ParentId = x.ParentId,
                    TenantId = x.TenantId,
                })
            );
            await tenantDbContext.SaveChangesAsync();

            // remove
            var tenantId = EngineContext.Current.ClaimManager.GetTenantId().To<Guid>();
            tenantDbContext.UnionQuestions.RemoveRange(
                tenantDbContext.UnionQuestions.AsNoTracking()
                .Where(x => x.TenantId == tenantId)
                .Where(x => !tenantDbContext.Questions.Any(y => y.Id == x.Id && y.TenantId == x.TenantId))
            );
            await tenantDbContext.SaveChangesAsync();
        }
    }
}