using HaoKao.Common.Enums;
using HaoKao.UserAnswerRecordService.Application.Modules.AnswerRecordModule;
using HaoKao.UserAnswerRecordService.Domain.Entities;
using HaoKao.UserAnswerRecordService.Domain.Queries;
using HaoKao.UserAnswerRecordService.Domain.Repositories;

namespace HaoKao.UserAnswerRecordService.Application.Modules.ProductRecordModule;

public interface IProductKnowledgeAnswerRecordService : IManager
{
    Task<AnswerRecordViewModel> Get(Guid id);

    Task<QueryProductKnowledgeListViewModel> GetList(QueryProductKnowledgeListViewModel viewModel);

    Task<KnowledgeMasteryStat> GetKnowledgeMasteryStat(QueryProductKnowledgeListViewModel viewModel);

    Task<ExamFrequencyMasteryViewModel> GetExamFrequencyMastery(Guid productId, Guid subjectId);

    Task<SubjectAnswerStatViewModel> GetSubjectAnswerStat(Guid productId, Guid subjectId);
}

public class ProductKnowledgeAnswerRecordService(
    IMapper mapper,
    IStaticCacheManager cacheManager,
    IProductKnowledgeAnswerRecordRepository repository
) : IProductKnowledgeAnswerRecordService
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
        var cacheKey = GirvsEntityCacheDefaults<ProductKnowledgeAnswerRecord>.ByIdCacheKey.Create(id.ToString());
        var entity = await cacheManager.GetAsync(cacheKey, () => repository.GetById(id));
        return mapper.Map<AnswerRecordViewModel>(entity.AnswerRecord);
    }

    /// <summary>
    /// 获取产品章节知识点记录
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    public async Task<QueryProductKnowledgeListViewModel> GetList(QueryProductKnowledgeListViewModel viewModel)
    {
        var query = viewModel.MapToQuery<ProductKnowledgeAnswerRecordQuery>();
        query.CreatorId = _userId;
        var key = $"knowledgePointList:{query.GetCacheKey()}";
        var cacheKey = GirvsEntityCacheDefaults<ProductKnowledgeAnswerRecord>.QueryCacheKey.Create(key);
        var temp = await cacheManager.GetAsync(cacheKey, async () =>
        {
            await repository.GetKnowledgePointList(query);
            return query;
        });
        if (!query.Equals(temp))
        {
            query.RecordCount = temp.RecordCount;
            query.Result = temp.Result;
        }

        // await repository.GetKnowledgePointList(query);
        return query.MapToQueryDto<QueryProductKnowledgeListViewModel, ProductKnowledgeAnswerRecord>();
    }

    /// <summary>
    /// 获取知识点掌握情况
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    public Task<KnowledgeMasteryStat> GetKnowledgeMasteryStat(QueryProductKnowledgeListViewModel viewModel)
    {
        var query = viewModel.MapToQuery<ProductKnowledgeAnswerRecordQuery>();
        query.CreatorId = _userId;
        var key = $"knowledgeMasteryStat:{query.GetCacheKey()}";
        var cacheKey = GirvsEntityCacheDefaults<ProductKnowledgeAnswerRecord>.QueryCacheKey.Create(key);

        return cacheManager.GetAsync(cacheKey, async () =>
        {
            var dict = await repository.GetKnowledgeMasteryStat(query);

            var masteredCount = dict.GetValueOrDefault(MasteryLevel.Mastered);
            var needsImprovementCount = dict.GetValueOrDefault(MasteryLevel.NeedsImprovement);
            var notMasteredCount = dict.GetValueOrDefault(MasteryLevel.NotMastered);
            var total = masteredCount + needsImprovementCount + notMasteredCount;

            return new KnowledgeMasteryStat
            {
                Mastered = masteredCount,
                NeedsImprovement = needsImprovementCount,
                NotMastered = notMasteredCount,
                Total = total,
            };
        });
    }

    /// <summary>
    /// 获取获取考试频率掌握情况
    /// </summary>
    /// <param name="productId">产品Id</param>
    /// <param name="subjectId">科目Id</param>
    /// <returns></returns>
    public Task<ExamFrequencyMasteryViewModel> GetExamFrequencyMastery(Guid productId, Guid subjectId)
    {
        var key = $"{nameof(GetExamFrequencyMastery)}:userId_{_userId}:productId_{productId}:subjectId_{subjectId}";
        var cacheKey = GirvsEntityCacheDefaults<ProductKnowledgeAnswerRecord>.QueryCacheKey.Create(key);
        return cacheManager.GetAsync(cacheKey, async () =>
        {
            var statsData = await repository.GetExamFrequencyMastery(_userId, productId, subjectId);
            var viewModel = new ExamFrequencyMasteryViewModel();

            var frequencyGroups = statsData.GroupBy(s => s.Frequency);

            foreach (var group in frequencyGroups)
            {
                var statDetail = new ExamFrequencyMasteryDetail
                {
                    MasteredCount = group.Count(g => g.Mastery == MasteryLevel.Mastered),
                    NotMasteredCount = group.Count(g => g.Mastery != MasteryLevel.Mastered)
                };

                switch (group.Key)
                {
                    case ExamFrequency.High: viewModel.High = statDetail; break;
                    case ExamFrequency.Medium: viewModel.Medium = statDetail; break;
                    case ExamFrequency.Low: viewModel.Low = statDetail; break;
                }
            }
            return viewModel;
        });
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
        var cacheKey = GirvsEntityCacheDefaults<ProductKnowledgeAnswerRecord>.QueryCacheKey.Create(key);
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