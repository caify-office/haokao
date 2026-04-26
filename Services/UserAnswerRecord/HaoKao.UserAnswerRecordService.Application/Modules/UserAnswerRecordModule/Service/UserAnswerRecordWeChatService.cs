using HaoKao.UserAnswerRecordService.Application.Modules.AnswerRecordModule;
using HaoKao.UserAnswerRecordService.Application.Modules.ProductRecordModule;
using HaoKao.UserAnswerRecordService.Application.Modules.UserAnswerRecordModule.Interfaces;
using HaoKao.UserAnswerRecordService.Application.Modules.UserAnswerRecordModule.ViewModels;
using HaoKao.UserAnswerRecordService.Domain.Entities;
using HaoKao.UserAnswerRecordService.Domain.Repositories;

namespace HaoKao.UserAnswerRecordService.Application.Modules.UserAnswerRecordModule.Service;

/// <summary>
/// 用户答题记录服务--小程序
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class UserAnswerRecordWeChatService(
    IChapterAnswerRecordService chapterService,
    IPaperAnswerRecordService paperService,
    IDateAnswerRecordService dateService,
    IDataAnalysisService analysisService,
    IProductChapterAnswerRecordService productChapterService,
    IProductPaperAnswerRecordService productPaperService,
    IProductKnowledgeAnswerRecordService productKnowledgeService,
    IElapsedTimeRecordRepository elapsedTimeRepository
) : IUserAnswerRecordWeChatService
{
    #region 题库首页

    /// <summary>
    /// 查询用户最新的练习记录Id
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<Guid> GetUserLatestRecordId()
    {
        return Task.FromResult(Guid.Empty);
    }

    /// <summary>
    /// 查询题库首页章节试题统计
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}"), AllowAnonymous]
    public Task<ChapterRecordStat> GetChapterRecordStat(Guid subjectId)
    {
        if (!EngineContext.Current.IsAuthenticated)
        {
            return Task.FromResult(new ChapterRecordStat(0, 0, 0, 0));
        }
        return chapterService.GetChapterRecordStat(subjectId);
    }

    /// <summary>
    /// 获取试卷列表
    /// </summary>
    /// <param name="paperIds">试卷Ids</param>
    /// <returns></returns>
    [HttpPost]
    public Task<IReadOnlyList<PaperRecordViewModel>> GetPaperList(IReadOnlyList<Guid> paperIds)
    {
        if (!EngineContext.Current.IsAuthenticated)
        {
            return Task.FromResult<IReadOnlyList<PaperRecordViewModel>>([]);
        }
        return paperService.GetPaperList(paperIds);
    }

    #endregion

    /// <summary>
    /// 获取章节记录
    /// </summary>
    /// <param name="categoryId">类别Id</param>
    /// <param name="chapterId">章节Id</param>
    /// <returns></returns>
    [HttpGet("{categoryId:guid}/{chapterId:guid}")]
    public Task<AnswerRecordViewModel> GetChapterAnswerRecord(Guid categoryId, Guid chapterId)
    {
        return chapterService.GetChapterAnswerRecord(categoryId, chapterId);
    }

    /// <summary>
    /// 获取小节记录
    /// </summary>
    /// <param name="categoryId">类别Id</param>
    /// <param name="sectionId">小节Id</param>
    /// <returns></returns>
    [HttpGet("{categoryId:guid}/{sectionId:guid}")]
    public Task<AnswerRecordViewModel> GetSectionAnswerRecord(Guid categoryId, Guid sectionId)
    {
        return chapterService.GetSectionAnswerRecord(categoryId, sectionId);
    }

    /// <summary>
    /// 获取知识点记录
    /// </summary>
    /// <param name="categoryId">类别Id</param>
    /// <param name="knowledgePointId">知识点Id</param>
    /// <returns></returns>
    [HttpGet("{categoryId:guid}/{knowledgePointId:guid}")]
    public Task<AnswerRecordViewModel> GetKnowledgePointAnswerRecord(Guid categoryId, Guid knowledgePointId)
    {
        return chapterService.GetKnowledgePointAnswerRecord(categoryId, knowledgePointId);
    }

    /// <summary>
    /// 获取试卷记录
    /// </summary>
    /// <param name="id">id</param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public Task<PaperAnswerRecordViewModel> GetPaperRecord(Guid id)
    {
        return paperService.Get(id);
    }

    /// <summary>
    /// 获取章节列表
    /// </summary>
    /// <param name="categoryId">类别Id</param>
    /// <param name="subjectId">科目Id</param>
    /// <returns></returns>
    [HttpGet("{categoryId:guid}/{subjectId:guid}")]
    public Task<IReadOnlyList<ChapterRecordViewModel>> GetChapterList(Guid categoryId, Guid subjectId)
    {
        return chapterService.GetChapterList(categoryId, subjectId);
    }

    /// <summary>
    /// 获取小节列表
    /// </summary>
    /// <param name="categoryId">类别Id</param>
    /// <param name="chapterId">章节Id</param>
    /// <returns></returns>
    [HttpGet("{categoryId:guid}/{chapterId:guid}")]
    public Task<IReadOnlyList<SectionRecordViewModel>> GetSectionList(Guid categoryId, Guid chapterId)
    {
        return chapterService.GetSectionList(categoryId, chapterId);
    }

    /// <summary>
    /// 获取知识点列表
    /// </summary>
    /// <param name="categoryId">类别Id</param>
    /// <param name="sectionId">小节Id</param>
    /// <returns></returns>
    [HttpGet("{categoryId:guid}/{sectionId:guid}")]
    public Task<IReadOnlyList<KnowledgePointRecordViewModel>> GetKnowledgePointList(Guid categoryId, Guid sectionId)
    {
        return chapterService.GetKnowledgePointList(categoryId, sectionId);
    }

    /// <summary>
    /// 获取试卷列表
    /// </summary>
    /// <param name="categoryId">类别Id</param>
    /// <param name="subjectId">科目Id</param>
    /// <returns></returns>
    [HttpGet("{categoryId:guid}/{subjectId:guid}")]
    public Task<IReadOnlyList<PaperRecordViewModel>> GetPaperList(Guid categoryId, Guid subjectId)
    {
        return paperService.GetPaperList(categoryId, subjectId);
    }

    /// <summary>
    /// 查询今日任务的练习记录Id
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}")]
    public Task<Guid> GetTodayTaskRecordId(Guid subjectId)
    {
        return dateService.GetTodayTaskRecordId(subjectId);
    }

    /// <summary>
    /// 获取今日任务答题记录
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public Task<AnswerRecordViewModel> GetTodayTaskRecord(Guid id)
    {
        return dateService.Get(id);
    }

    /// <summary>
    /// 获取每日打卡记录(月)
    /// </summary>
    /// <param name="subjectId">科目Id</param>
    /// <param name="date">日期</param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}")]
    public Task<IReadOnlyList<DateOnly>> GetDailyRecordList(Guid subjectId, DateOnly date)
    {
        return dateService.GetDailyRecordList(subjectId, date);
    }

    /// <summary>
    /// 获取每日打卡排名占比(月)
    /// </summary>
    /// <param name="subjectId">科目Id</param>
    /// <param name="date">日期</param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}")]
    public Task<decimal> GetDailyRankingRatio(Guid subjectId, DateOnly date)
    {
        return dateService.GetDailyRankingRatio(subjectId, date);
    }

    /// <summary>
    /// 每日一题打卡人数统计(每日00:00更新)
    /// </summary>
    /// <param name="subjectId">科目Id</param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}")]
    public Task<int> GetDailyUserCount(Guid subjectId)
    {
        return dateService.GetDailyUserCount(subjectId);
    }

    /// <summary>
    /// 获取做题能力分析
    /// </summary>
    /// <param name="subjectId"></param>
    [HttpGet("{subjectId:guid}")]
    public Task<PracticeAbilityAnalysisViewModel> GetPracticeAbilityAnalysis(Guid subjectId)
    {
        return analysisService.GetPracticeAbilityAnalysis(subjectId);
    }

    /// <summary>
    /// 刷新做题能力分析
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}")]
    public ValueTask RefreshPracticeAbilityAnalysis(Guid subjectId)
    {
        return analysisService.RefreshPracticeAbilityAnalysis(subjectId);
    }

    /// <summary>
    /// 获取练习状况分析
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}")]
    public Task<PracticeSituationAnalysisViewModel> GetPracticeSituationAnalysis(Guid subjectId)
    {
        return analysisService.GetPracticeSituationAnalysis(subjectId);
    }

    /// <summary>
    /// 刷新练习状况分析
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}")]
    public ValueTask RefreshPracticeSituationAnalysis(Guid subjectId)
    {
        return analysisService.RefreshPracticeSituationAnalysis(subjectId);
    }

    #region 智辅课程

    /// <summary>
    /// 按Id获取产品章节记录(答题结果页面)
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public Task<AnswerRecordViewModel> GetProductChapterRecord(Guid id)
    {
        return productChapterService.Get(id);
    }

    /// <summary>
    /// 获取章节作答记录Ids
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<Dictionary<Guid, Guid>> GetProductChapterRecordIds([FromBody] QueryChapterRecordIdsViewModel viewModel)
    {
        return productChapterService.GetChapterRecordIds(viewModel);
    }

    /// <summary>
    /// 按Id获取产品试卷记录(答题结果页面)
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public Task<AnswerRecordViewModel> GetProductPaperRecord(Guid id)
    {
        return productPaperService.Get(id);
    }

    /// <summary>
    /// 获取试卷作答记录Ids
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<Dictionary<Guid, Guid>> GetProductPaperRecordIds([FromBody] QueryPaperRecordIdsViewModel viewModel)
    {
        return productPaperService.GetPaperRecordIds(viewModel);
    }

    /// <summary>
    /// 按Id获取产品知识点记录(答题结果页面)
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public Task<AnswerRecordViewModel> GetProductKnowledgeRecord(Guid id)
    {
        return productKnowledgeService.Get(id);
    }

    /// <summary>
    /// 获取产品知识点记录列表
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<QueryProductKnowledgeListViewModel> GetProductKnowledgeList([FromBody] QueryProductKnowledgeListViewModel viewModel)
    {
        return productKnowledgeService.GetList(viewModel);
    }

    /// <summary>
    /// 获取知识点掌握情况
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<KnowledgeMasteryStat> GetKnowledgeMasteryStat([FromBody] QueryProductKnowledgeListViewModel viewModel)
    {
        return productKnowledgeService.GetKnowledgeMasteryStat(viewModel);
    }

    /// <summary>
    /// 获取获取考试频率掌握情况
    /// </summary>
    /// <param name="productId">产品Id</param>
    /// <param name="subjectId">科目Id</param>
    /// <returns></returns>
    [HttpGet("Product/{productId:guid}/Subject/{subjectId:guid}")]
    public Task<ExamFrequencyMasteryViewModel> GetExamFrequencyMastery(Guid productId, Guid subjectId)
    {
        return productKnowledgeService.GetExamFrequencyMastery(productId, subjectId);
    }

    /// <summary>
    /// 获取科目作答统计(做题数,正确数和总数)
    /// </summary>
    /// <param name="productId">产品Id</param>
    /// <param name="subjectId">科目Id</param>
    /// <returns></returns>
    [HttpGet("Product/{productId:guid}/Subject/{subjectId:guid}")]
    public async Task<SubjectAnswerStatViewModel> GetSubjectAnswerStat(Guid productId, Guid subjectId)
    {
        var data1 = await productChapterService.GetSubjectAnswerStat(productId, subjectId);
        var data2 = await productPaperService.GetSubjectAnswerStat(productId, subjectId);
        var data3 = await productKnowledgeService.GetSubjectAnswerStat(productId, subjectId);
        return new SubjectAnswerStatViewModel
        {
            QuestionCount = data1.QuestionCount + data2.QuestionCount + data3.QuestionCount,
            AnswerCount = data1.AnswerCount + data2.AnswerCount + data3.AnswerCount,
            CorrectCount = data1.CorrectCount + data2.CorrectCount + data3.CorrectCount,
        };
    }

    /// <summary>
    /// 按日期统计做题时长
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IReadOnlyList<DateStudyDurationViewModel>> GetDateStudyDuration([FromQuery] QueryDateStudyDurationViewModel viewModel)
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        var data = await elapsedTimeRepository.GetDateElaspedTime(userId, viewModel.ProductId, viewModel.SubjectId, viewModel.StartDate, viewModel.EndDate);

        var date = viewModel.StartDate;
        var list = new List<DateStudyDurationViewModel>();
        // 按照开始和结束序列化日期
        while (true)
        {
            list.Add(new DateStudyDurationViewModel
            {
                Date = date,
                Duration = Math.Round(data.FirstOrDefault(x => x.Date == date).TotalSeconds / 3600.0, 1)
            });

            if (date >= viewModel.EndDate)
            {
                break;
            }

            date = date.AddDays(1);
        }
        return list;
    }

    #endregion
}