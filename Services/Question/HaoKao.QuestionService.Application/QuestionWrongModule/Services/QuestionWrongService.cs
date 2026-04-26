using HaoKao.QuestionService.Application.QuestionModule.ViewModels;
using HaoKao.QuestionService.Application.QuestionWrongModule.Interfaces;
using HaoKao.QuestionService.Application.QuestionWrongModule.ViewModels;
using HaoKao.QuestionService.Domain.CacheExtensions;
using HaoKao.QuestionService.Domain.QuestionModule;
using HaoKao.QuestionService.Domain.QuestionWrongModule;
using HaoKao.QuestionService.Domain.Works;
using QueryChapterQuestionViewModel = HaoKao.QuestionService.Application.QuestionWrongModule.ViewModels.QueryChapterQuestionViewModel;
using QueryChapterViewModel = HaoKao.QuestionService.Application.QuestionWrongModule.ViewModels.QueryChapterViewModel;

namespace HaoKao.QuestionService.Application.QuestionWrongModule.Services;

[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
public class QuestionWrongService(
    IStaticCacheManager cacheManager,
    IQuestionWrongRepository repository
) : IQuestionWrongService
{
    private readonly Guid _userId = EngineContext.Current.IsAuthenticated ? EngineContext.Current.ClaimManager.GetUserId().To<Guid>() : Guid.Empty;

    /// <inheritdoc />
    [NonAction]
    public async Task<IReadOnlyList<ChapterViewModel>> GetChapterList(QueryChapterViewModel input)
    {
        var key = $"{nameof(GetChapterList)}:{JsonSerializer.Serialize(input).ToMd5()}";
        var cacheKey = QuestionWrongCacheManager.ListQuery.Create(key);
        return await cacheManager.GetAsync(cacheKey, async () =>
        {
            var list = await repository.GetChapterList(_userId, input.SubjectId, input.IsActive);
            return list.Where(x => x.Count > 0)
                       .Select(x => new ChapterViewModel(x.Id, x.Name, x.Count))
                       .ToList();
        });
    }

    /// <inheritdoc />
    [NonAction]
    public Task<IReadOnlyList<Question>> GetChapterQuestionList(QueryChapterQuestionViewModel input)
    {
        var cacheKey = $"userId_{_userId}:{nameof(GetChapterQuestionList)}:{JsonSerializer.Serialize(input).ToMd5()}";
        return cacheManager.GetAsync(
            QuestionWrongCacheManager.ListQuery.Create(cacheKey),
            () => repository.GetChapterQuestionList(_userId, input.ChapterId, input.IsActive)
        );
    }

    /// <summary>
    /// 根据科目判断用户是否存在未消灭的错题
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    [NonAction]
    public Task<bool> Any(Guid subjectId)
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        return cacheManager.GetAsync(
            QuestionWrongCacheManager.ListQuery.Create($"{nameof(Any)}:{subjectId}", subjectId.ToString()),
            () => repository.ExistEntityAsync(x => x.CreatorId == userId &&
                                                   x.Question.SubjectId == subjectId &&
                                                   x.IsActive)
        );
    }

    /// <summary>
    /// 根据科目查询今日任务的试题Ids
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [NonAction]
    public Task<List<Guid>> GetTodayTaskQuestionIds(QueryTodayTaskViewModel input)
    {
        // cacheTime 单位是分钟
        var cacheTime = (DateTime.Today.AddDays(1) - DateTime.Now).TotalMinutes + TimeSpan.FromHours(6).TotalMinutes;
        var prefix = $"{QuestionWrongCacheManager.Prefix.Key}:{nameof(GetTodayTaskQuestionIds)}:{input.SubjectId}:{DateTime.Now:yyyy-MM-dd}";
        return cacheManager.GetAsync(
            new CacheKey(prefix).Create(cacheTime: (int)cacheTime),
            () => repository.GetTodayTaskQuestionIds(input.SubjectId, input.Count)
        );
    }

    /// <summary>
    /// 获取错题数量
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<Dictionary<string, int>> GetQuestionWrongCount(Guid subjectId, Guid userId)
    {
        var query = repository.Query.Where(x => x.CreatorId == userId && x.Question.SubjectId == subjectId);
        return new Dictionary<string, int>
        {
            ["ActiveCount"] = await query.CountAsync(x => x.IsActive),
            ["InactiveCount"] = await query.CountAsync(x => !x.IsActive),
        };
    }

    [HttpGet, AllowAnonymous]
    public Task CleanDuplicateQuestionWrong([FromServices] ICleanDuplicateQuestionWrongWork work)
    {
        return work.ExecuteAsync();
    }

    [HttpGet, AllowAnonymous]
    public Task InitQuestionWrongSort([FromServices] IInitQuestionWrongSortWork work)
    {
        return work.ExecuteAsync();
    }
}