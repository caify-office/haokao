using HaoKao.UserAnswerRecordService.Application.Modules.AnswerRecordModule;
using HaoKao.UserAnswerRecordService.Domain.Entities;
using HaoKao.UserAnswerRecordService.Domain.Repositories;

namespace HaoKao.UserAnswerRecordService.Application.Modules.ProductRecordModule;

public interface IProductPaperAnswerRecordService : IManager
{
    Task<AnswerRecordViewModel> Get(Guid id);

    Task<Dictionary<Guid, Guid>> GetPaperRecordIds(QueryPaperRecordIdsViewModel viewModel);

    Task<SubjectAnswerStatViewModel> GetSubjectAnswerStat(Guid productId, Guid subjectId);
}

public class ProductPaperAnswerRecordService(
    IMapper mapper,
    IStaticCacheManager cacheManager,
    IProductPaperAnswerRecordRepository repository
) : IProductPaperAnswerRecordService
{
    private readonly Guid _userId = EngineContext.Current.IsAuthenticated
        ? EngineContext.Current.ClaimManager.GetUserId().To<Guid>()
        : Guid.Empty;

    /// <summary>
    /// 按Id获取记录(答题结果页面)
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<AnswerRecordViewModel> Get(Guid id)
    {
        var cacheKey = GirvsEntityCacheDefaults<ProductPaperAnswerRecord>.ByIdCacheKey.Create(id.ToString());
        var entity = await cacheManager.GetAsync(cacheKey, () => repository.GetById(id));
        return mapper.Map<AnswerRecordViewModel>(entity.AnswerRecord);
    }

    /// <summary>
    /// 获取试卷作答记录Ids
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    public Task<Dictionary<Guid, Guid>> GetPaperRecordIds(QueryPaperRecordIdsViewModel viewModel)
    {
        var key = $"userId:{_userId}:{JsonSerializer.Serialize(viewModel).ToMd5()}";
        var cacheKey = GirvsEntityCacheDefaults<ProductPaperAnswerRecord>.QueryCacheKey.Create(key);
        return cacheManager.GetAsync(cacheKey, () => repository.GetPaperRecordIds(_userId, viewModel.ProductId, viewModel.PaperIds));
        // return repository.GetPaperRecordIds(_userId, viewModel.ProductId, viewModel.PaperIds);
    }

    /// <summary>
    /// 获取科目作答统计(做题数,正确数和总数)
    /// </summary>
    /// <param name="productId">产品Id</param>
    /// <param name="subjectId">科目Id</param>
    /// <returns></returns>
    public Task<SubjectAnswerStatViewModel> GetSubjectAnswerStat(Guid productId, Guid subjectId)
    {
        var key = $"{nameof(GetSubjectAnswerStat)}:userId_{_userId}:productId_{productId}:subjectId_{subjectId}";
        var cacheKey = GirvsEntityCacheDefaults<ProductPaperAnswerRecord>.QueryCacheKey.Create(key);
        return cacheManager.GetAsync(cacheKey, async () =>
        {
            var stat = await repository.GetSubjectAnswerStat(_userId, productId, subjectId);
            return new SubjectAnswerStatViewModel
            {
                QuestionCount = stat.QuestionCount,
                AnswerCount = stat.AnswerCount,
                CorrectCount = stat.CorrectCount,
            };
        });
    }
}