using Girvs.Driven.Extensions;
using Girvs.Extensions;
using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.LearnProgressService.Application.AppService.WeChat;
using HaoKao.LearnProgressService.Application.ViewModels;
using HaoKao.LearnProgressService.Application.ViewModels.DailyStudyDuration;
using HaoKao.LearnProgressService.Domain.Commands.DailyStudyDuration;
using HaoKao.LearnProgressService.Domain.Repositories;
using Microsoft.AspNetCore.Hosting;
using System.Linq;

namespace HaoKao.LearnProgressService.Application.AppService.Web;

/// <summary>
/// 接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class DailyStudyDurationWebService(
        IMediatorHandler bus,
        INotificationHandler<DomainNotification> notifications,
        IDailyStudyDurationRepository repository
    ) : IDailyStudyDurationWebService
{
    #region 初始参数
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IDailyStudyDurationRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 按产品,科目id获取当前用户学习天数
    /// </summary>
    /// <returns></returns>
    [HttpGet("{productId}/{subjectId}")]
    public async Task<int> GetLearnDayCount(Guid productId, Guid subjectId)
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().ToGuid();
        int result = await _repository.GetLearnDayCount(productId,subjectId,userId);
        return result;
    }
    /// <summary>
    /// 按产品id,科目id,时间段获取当前学习时长
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IReadOnlyList<DateStudyDurationViewModel>> GetDailyLearnData([FromBody] QueryDailyLearnDataViewModel model)
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().ToGuid();
        var result = await _repository.GetWhereAsync(x =>
        x.CreatorId == userId
        && x.ProductId == model.ProductId
        && x.SubjectId == model.SubjectId
        && x.LearnTime >= model.StartDate
        && x.LearnTime <= model.EndDate
        );
        var learnDic= result.GroupBy(x=>x.LearnTime).ToDictionary(x => x.Key, x => x.Last().DailyVideoStudyDuration);
        var list = new List<DateStudyDurationViewModel>();
        var time = model.StartDate;
        while (time <= model.EndDate)
        {
            var studyDuration = new DateStudyDurationViewModel
            {
                Date = time,
                Duration= learnDic.ContainsKey(time) ? learnDic[time] : 0
            };
            list.Add(studyDuration);
            time=time.AddDays(1);
        }
        return list;
    }
    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public async Task Create([FromBody]CreateDailyStudyDurationViewModel model)
    {
        var command = model.MapToCommand<CreateDailyStudyDurationCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }



    #endregion
}