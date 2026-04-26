using HaoKao.LearnProgressService.Application.ViewModels;
using HaoKao.LearnProgressService.Application.ViewModels.DailyStudyDuration;

namespace HaoKao.LearnProgressService.Application.AppService.Web;

public interface IDailyStudyDurationWebService : IAppWebApiService, IManager
{
    /// <summary>
    /// 获取当前用户学习天数
    /// </summary>
    /// <returns></returns>

    Task<int> GetLearnDayCount(Guid productId, Guid subjectId);
    /// <summary>
    /// 按时间段获取学习时长
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>

    Task<IReadOnlyList<DateStudyDurationViewModel>> GetDailyLearnData(QueryDailyLearnDataViewModel model);
     /// <summary>
     /// 创建
     /// </summary>
     /// <param name="model">新增模型</param>
     Task Create(CreateDailyStudyDurationViewModel model);

}