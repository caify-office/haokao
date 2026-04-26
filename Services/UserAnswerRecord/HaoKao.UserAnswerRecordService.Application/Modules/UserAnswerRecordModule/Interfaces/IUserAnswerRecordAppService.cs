using HaoKao.UserAnswerRecordService.Application.Modules.UserAnswerRecordModule.ViewModels;

namespace HaoKao.UserAnswerRecordService.Application.Modules.UserAnswerRecordModule.Interfaces;

public interface IUserAnswerRecordAppService : IManager, IAppWebApiService
{
    /// <summary>
    /// 按Id获取记录(答题结果页面)
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<UserAnswerRecordAppViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询条件获取指定的答题记录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<UserAnswerRecordQueryViewModel> Get(UserAnswerRecordQueryViewModel input);

    /// <summary>
    /// 获取章节列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<List<UserAnswerRecordChapterListAppViewModel>> GetChapterList(UserAnswerRecordAppQueryModel input);

    /// <summary>
    /// 获取试卷列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<List<UserAnswerRecordPaperListAppViewModel>> GetPaperList(UserAnswerRecordAppQueryModel input);

    /// <summary>
    /// 查询用户最新的练习记录Id
    /// </summary>
    /// <returns></returns>
    Task<Guid> GetUserLatestRecordId();

    /// <summary>
    /// 查询题库首页试题统计
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    Task<ChapterRecordStatViewModel> GetChapterRecordStat(Guid subjectId);

    /// <summary>
    /// 查询题库首页试卷详细情况
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<List<PaperAnswerDetailAppViewModel>> GetUserPaperDetail(UserAnswerRecordAppQueryModel input);

    /// <summary>
    /// 题库首页整卷测验统计接口以及首页点击进去列表查询接口
    /// </summary>
    /// <param name="paperIds"></param>
    /// <returns></returns>
    Task<List<AppHomePaperAnswerViewModel>> GetHomePaperAnswerDetail(Guid[] paperIds);

    /// <summary>
    /// 查询当月每日一题打卡记录
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    Task<List<DateTime>> GetPunchInRecordList(Guid subjectId, DateTime dateTime);

    /// <summary>
    /// 获取打卡排名比例
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    Task<decimal> GetPunchInRankingRatio(Guid subjectId, DateTime dateTime);

    /// <summary>
    /// 每日一题打卡人数统计(每日00:00更新)
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    Task<int> GetJoinPunchInPeopleNumber(Guid subjectId);

    /// <summary>
    /// 查询今日任务的练习记录Id
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    Task<Guid> GetTodayTaskRecordId(Guid subjectId);

    /// <summary>
    /// 获取做题能力分析
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<PracticeAbilityAnalysisViewModel> GetPracticeAbilityAnalysis(Guid subjectId, Guid? userId = null);

    /// <summary>
    /// 刷新做题能力分析
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    ValueTask RefreshPracticeAbilityAnalysis(Guid subjectId, Guid? userId = null);

    /// <summary>
    /// 获取练习状况分析
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<PracticeSituationAnalysisViewModel> GetPracticeSituationAnalysis(Guid subjectId, Guid? userId = null);

    /// <summary>
    /// 刷新练习状况分析
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    ValueTask RefreshPracticeSituationAnalysis(Guid subjectId, Guid? userId = null);
}