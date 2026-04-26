using HaoKao.QuestionService.Domain.QuestionModule;
using HaoKao.QuestionService.Domain.Works;
using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;

namespace HaoKao.QuestionService.Infrastructure.Works;

public class InitQuestionTitleWork(IServiceProvider provider) : IInitQuestionTitleWork
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

            var total = await tenantDbContext.Questions.CountAsync();
            const int pageSize = 1000;
            var pageCount = (int)Math.Ceiling(total / (double)pageSize);

            for (var i = 0; i < pageCount; i++)
            {
                // 使用HtmlAgilityPack库提取QuestionText中的文本内容, 截取前15个字符, 赋值给QuestionTitle
                var questions = await tenantDbContext.Questions.Skip(i * pageSize).Take(pageSize).ToListAsync();
                foreach (var question in questions)
                {
                    var questionText = question.QuestionText;
                    if (string.IsNullOrWhiteSpace(questionText) && !string.IsNullOrEmpty(question.QuestionTitle))
                    {
                        continue;
                    }

                    var doc = new HtmlDocument();
                    doc.LoadHtml(questionText);
                    var questionTitle = doc.DocumentNode.InnerText.Trim();
                    questionTitle = questionTitle.Length > 15 ? questionTitle[..15] : questionTitle;
                    question.QuestionTitle = questionTitle;
                }

                await tenantDbContext.SaveChangesAsync();
            }
        }
    }
}

public class InitQuestionCountWork(IServiceProvider provider) : IInitQuestionCountWork
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

            var questions = await tenantDbContext.Questions.Where(x => x.ParentId.HasValue).ToListAsync();
            foreach (var question in questions)
            {
                question.QuestionCount = 0;
            }
            await tenantDbContext.SaveChangesAsync();

            var parentIds = questions.Select(x => x.ParentId).Distinct().ToList();
            questions = await tenantDbContext.Questions.Where(x => parentIds.Contains(x.Id)).ToListAsync();
            foreach (var question in questions)
            {
                question.QuestionCount = questions.Count(x => x.ParentId == question.Id);
            }
            await tenantDbContext.SaveChangesAsync();
        }
    }
}