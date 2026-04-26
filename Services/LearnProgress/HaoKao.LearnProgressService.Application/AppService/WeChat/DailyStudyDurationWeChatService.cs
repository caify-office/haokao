using Girvs.EntityFrameworkCore.Repositories;
using Girvs.Extensions;
using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.LearnProgressService.Application.AppService.Web;
using HaoKao.LearnProgressService.Application.ViewModels;
using HaoKao.LearnProgressService.Application.ViewModels.DailyStudyDuration;
using HaoKao.LearnProgressService.Domain.Commands.DailyStudyDuration;
using HaoKao.LearnProgressService.Domain.Repositories;
using System.Linq;

namespace HaoKao.LearnProgressService.Application.AppService.WeChat;

/// <summary>
/// 接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class DailyStudyDurationWeChatService( IDailyStudyDurationWebService  dailyStudyDurationWebService ) : IDailyStudyDurationWeChatService
{
    #region 服务方法

    /// <summary>
    /// 按产品id,科目id获取当前用户学习天数
    /// </summary>
    /// <returns></returns>
    [HttpGet("{productId}/{subjectId}")]
    public  Task<int> GetLearnDayCount(Guid productId, Guid subjectId)
    {
       return dailyStudyDurationWebService.GetLearnDayCount(productId, subjectId);
    }
    /// <summary>
    /// 按产品id,科目id,时间段获取当前学习时长
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public  Task<IReadOnlyList<DateStudyDurationViewModel>> GetDailyLearnData([FromBody] QueryDailyLearnDataViewModel model)
    {
        return dailyStudyDurationWebService.GetDailyLearnData(model);
    }
    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public  Task Create([FromBody] CreateDailyStudyDurationViewModel model)
    {
        return dailyStudyDurationWebService.Create(model);
    }
    #endregion
}