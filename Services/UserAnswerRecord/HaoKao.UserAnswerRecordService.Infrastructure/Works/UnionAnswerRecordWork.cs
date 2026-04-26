using HaoKao.Common.Enums;
using HaoKao.UserAnswerRecordService.Domain.Entities;
using HaoKao.UserAnswerRecordService.Domain.Works;
using Microsoft.Extensions.DependencyInjection;

namespace HaoKao.UserAnswerRecordService.Infrastructure.Works;

public class UnionAnswerRecordWork(IServiceProvider provider) : IUnionAnswerRecordWork
{
    public async Task ExecuteAsync()
    {
        await using var scope = provider.CreateAsyncScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<UserAnswerRecordDbContext>();
        var tables = await dbContext.GetTableNameList(nameof(UserAnswerRecord));
        var hashSet = new HashSet<string>(tables.Count);
        foreach (var table in tables.Where(x => x.Length > nameof(UserAnswerRecord).Length + 5))
        {
            await using var tenantScope = provider.CreateTenantAsyncScope(table);
            await using var tenantDbContext = tenantScope.ServiceProvider.GetRequiredService<UserAnswerRecordDbContext>();
            tenantDbContext.ShardingAutoMigration();

            // 避免重复执行相同租户
            var tenantId = EngineContext.Current.ClaimManager.GetTenantId();
            if (!hashSet.Add(tenantId)) continue;

            tenantDbContext.UnionAnswerRecords.AddRange(
                tenantDbContext.UserAnswerRecords.AsNoTracking()
                               .Where(x => !tenantDbContext.UnionAnswerRecords.Any(y => y.Id == x.Id))
                               .Select(x => new UnionAnswerRecord
                               {
                                   Id = x.Id,
                                   SubjectId = x.SubjectId,
                                   RecordIdentifier = x.RecordIdentifier,
                                   QuestionCategoryId = x.QuestionCategoryId,
                                   CreateTime = x.CreateTime,
                                   CreatorId = x.CreatorId,
                                   TenantId = x.TenantId,
                               }));
            await tenantDbContext.SaveChangesAsync();

            tenantDbContext.UnionAnswerQuestions.AddRange(
                tenantDbContext.UserAnswerQuestions.AsNoTracking()
                               .Where(x => !tenantDbContext.UnionAnswerQuestions.Any(y => y.Id == x.Id))
                               .Select(x => new UnionAnswerQuestion
                               {
                                   Id = x.Id,
                                   UnionAnswerRecordId = x.UserAnswerRecordId,
                                   QuestionId = x.QuestionId,
                                   QuestionTypeId = x.QuestionTypeId,
                                   ParentId = x.ParentId,
                                   JudgeResult = x.JudgeResult,
                                   TenantId = x.TenantId,
                               }));
            await tenantDbContext.SaveChangesAsync();
        }
    }
}

public class MigrateRecordDataWork(IServiceProvider provider) : IMigrateRecordDataWork
{
    public async Task ExecuteAsync()
    {
        await using var scope = provider.CreateAsyncScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<UserAnswerRecordDbContext>();
        var tables = await dbContext.GetTableNameList(nameof(UserAnswerRecord));
        var hashSet = new HashSet<string>(tables.Count);
        foreach (var table in tables.Where(x => x.Length > nameof(UserAnswerRecord).Length + 5))
        {
            // if (!table.Contains("08db5bf25b79498b8145de0c5aae3271")) continue;
            // if (!table.Contains("08db5bf2afae4d40889618e7e86b6b37")) continue;
            // if (!table.Contains("08db5bf29ec9492e8fb404b01d0f881f")) continue;

            await using var tenantScope = provider.CreateTenantAsyncScope(table);
            await using var tenantDbContext = tenantScope.ServiceProvider.GetRequiredService<UserAnswerRecordDbContext>();
            tenantDbContext.ShardingAutoMigration();

            // 避免重复执行相同租户
            var tenantId = EngineContext.Current.ClaimManager.GetTenantId();
            if (!hashSet.Add(tenantId)) continue;

            var chapters = await tenantDbContext.UserAnswerRecords.AsNoTracking()
                                                .Include(x => x.RecordQuestions).ThenInclude(x => x.QuestionOptions)
                                                .Where(x => x.AnswerType == SubmitAnswerType.Chapter && x.QuestionCategoryId != Guid.Empty)
                                                .Select(x => new ChapterAnswerRecord
                                                {
                                                    Id = x.Id,
                                                    SubjectId = x.SubjectId,
                                                    CategoryId = x.QuestionCategoryId,
                                                    ChapterId = x.RecordIdentifier,
                                                    SectionId = Guid.Empty,
                                                    KnowledgePointId = Guid.Empty,
                                                    CreatorId = x.CreatorId,
                                                    CreateTime = x.CreateTime,
                                                    TenantId = x.TenantId,
                                                    AnswerRecord = new AnswerRecord
                                                    {
                                                        SubjectId = x.SubjectId,
                                                        QuestionCount = x.QuestionCount,
                                                        AnswerCount = x.AnswerCount,
                                                        CorrectCount = x.CorrectCount,
                                                        AnswerType = x.AnswerType,
                                                        CreatorId = x.CreatorId,
                                                        CreateTime = x.CreateTime,
                                                        TenantId = x.TenantId,
                                                        Questions = x.RecordQuestions.Select(y => new AnswerQuestion
                                                        {
                                                            QuestionId = y.QuestionId,
                                                            QuestionTypeId = y.QuestionTypeId,
                                                            JudgeResult = y.JudgeResult,
                                                            WhetherMark = y.WhetherMark,
                                                            ParentId = y.ParentId,
                                                            CreatorId = x.CreatorId,
                                                            CreateTime = x.CreateTime,
                                                            TenantId = y.TenantId,
                                                            UserAnswers = y.QuestionOptions.Select(z => new UserAnswer(z.OptionId == Guid.Empty ? z.OptionContent : z.OptionId.ToString())).ToList(),
                                                        }).ToList()
                                                    }
                                                }).ToListAsync();
            await tenantDbContext.ChapterAnswerRecords.AddRangeAsync(chapters);

            var papers = await tenantDbContext.UserAnswerRecords.AsNoTracking()
                                              .Include(x => x.RecordQuestions).ThenInclude(x => x.QuestionOptions)
                                              .Where(x => x.AnswerType == SubmitAnswerType.Paper && x.QuestionCategoryId != Guid.Empty)
                                              .Select(x => new PaperAnswerRecord
                                              {
                                                  Id = x.Id,
                                                  SubjectId = x.SubjectId,
                                                  CategoryId = x.QuestionCategoryId,
                                                  PaperId = x.RecordIdentifier,
                                                  UserScore = x.UserScore,
                                                  PassingScore = x.PassingScore,
                                                  TotalScore = x.TotalScore,
                                                  CreatorId = x.CreatorId,
                                                  CreateTime = x.CreateTime,
                                                  TenantId = x.TenantId,
                                                  AnswerRecord = new AnswerRecord
                                                  {
                                                      SubjectId = x.SubjectId,
                                                      QuestionCount = x.QuestionCount,
                                                      AnswerCount = x.AnswerCount,
                                                      CorrectCount = x.CorrectCount,
                                                      AnswerType = x.AnswerType,
                                                      CreatorId = x.CreatorId,
                                                      CreateTime = x.CreateTime,
                                                      TenantId = x.TenantId,
                                                      Questions = x.RecordQuestions.Select(y => new AnswerQuestion
                                                      {
                                                          QuestionId = y.QuestionId,
                                                          QuestionTypeId = y.QuestionTypeId,
                                                          JudgeResult = y.JudgeResult,
                                                          WhetherMark = y.WhetherMark,
                                                          ParentId = y.ParentId,
                                                          CreatorId = x.CreatorId,
                                                          CreateTime = x.CreateTime,
                                                          TenantId = y.TenantId,
                                                          UserAnswers = y.QuestionOptions.Select(z => new UserAnswer(z.OptionId == Guid.Empty ? z.OptionContent : z.OptionId.ToString())).ToList(),
                                                      }).ToList()
                                                  }
                                              }).ToListAsync();
            await tenantDbContext.PaperAnswerRecords.AddRangeAsync(papers);

            var dates = await tenantDbContext.UserAnswerRecords.AsNoTracking()
                                             .Include(x => x.RecordQuestions).ThenInclude(x => x.QuestionOptions)
                                             .Where(x => x.AnswerType == SubmitAnswerType.Daily || x.AnswerType == SubmitAnswerType.TodayTask)
                                             .Select(x => new DateAnswerRecord
                                             {
                                                 Id = x.Id,
                                                 SubjectId = x.SubjectId,
                                                 Type = x.AnswerType,
                                                 Date = DateOnly.FromDateTime(x.CreateTime),
                                                 CreatorId = x.CreatorId,
                                                 CreateTime = x.CreateTime,
                                                 TenantId = x.TenantId,
                                                 AnswerRecord = new AnswerRecord
                                                 {
                                                     SubjectId = x.SubjectId,
                                                     QuestionCount = x.QuestionCount,
                                                     AnswerCount = x.AnswerCount,
                                                     CorrectCount = x.CorrectCount,
                                                     AnswerType = x.AnswerType,
                                                     CreatorId = x.CreatorId,
                                                     CreateTime = x.CreateTime,
                                                     TenantId = x.TenantId,
                                                     Questions = x.RecordQuestions.Select(y => new AnswerQuestion
                                                     {
                                                         QuestionId = y.QuestionId,
                                                         QuestionTypeId = y.QuestionTypeId,
                                                         JudgeResult = y.JudgeResult,
                                                         WhetherMark = y.WhetherMark,
                                                         ParentId = y.ParentId,
                                                         CreatorId = x.CreatorId,
                                                         CreateTime = x.CreateTime,
                                                         TenantId = y.TenantId,
                                                         UserAnswers = y.QuestionOptions.Select(z => new UserAnswer(z.OptionId == Guid.Empty ? z.OptionContent : z.OptionId.ToString())).ToList(),
                                                     }).ToList()
                                                 }
                                             }).ToListAsync();
            await tenantDbContext.DateAnswerRecords.AddRangeAsync(dates);

            var elapsedTimeRecords = await tenantDbContext.UserAnswerRecords.AsNoTracking()
                                                          .Select(x => new ElapsedTimeRecord
                                                          {
                                                              SubjectId = x.SubjectId,
                                                              TargetId = x.RecordIdentifier,
                                                              ProductId = Guid.Empty,
                                                              Type = x.AnswerType,
                                                              QuestionCount = x.QuestionCount,
                                                              AnswerCount = x.AnswerCount,
                                                              CorrectCount = x.CorrectCount,
                                                              ElapsedSeconds = x.ElapsedTime,
                                                              CreatorId = x.CreatorId,
                                                              CreateTime = x.CreateTime,
                                                              CreateDate = DateOnly.FromDateTime(x.CreateTime)
                                                          }).ToListAsync();
            await tenantDbContext.ElapsedTimeRecords.AddRangeAsync(elapsedTimeRecords);


            await tenantDbContext.SaveChangesAsync();
        }
    }
}