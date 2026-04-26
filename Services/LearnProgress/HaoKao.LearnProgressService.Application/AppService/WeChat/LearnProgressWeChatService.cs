


using Girvs.Extensions;
using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.LearnProgressService.Application.AppService.Web;
using HaoKao.LearnProgressService.Application.ViewModels.LearnProgress;
using HaoKao.LearnProgressService.Domain.Commands.LearnProgress;
using HaoKao.LearnProgressService.Domain.Entities;
using HaoKao.LearnProgressService.Domain.Queries.EntityQuery;
using HaoKao.LearnProgressService.Domain.Repositories;
using MediatR;
using System.Linq;

namespace HaoKao.LearnProgressService.Application.AppService.WeChat;

/// <summary>
/// 接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class LearnProgressWeChatService(
    ILearnProgressWebService learnProgressWebService,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    ILearnProgressRepository repository
) : ILearnProgressWeChatService
{
    #region 初始参数
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly ILearnProgressRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法
    /// <summary>
    /// 分产品分科目获取学习时长(单位小时)
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="subjectId"></param>
    [HttpGet("{productId}/{subjectId}")]
    public Task<float> GetLearnDurations(Guid productId, Guid subjectId)
    {
        return learnProgressWebService.GetLearnDurations(productId,subjectId);
    }
    /// <summary>
    /// 根据主键获取指定,这里应该是传的videoid查询当前视频的播放进度
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet]
    public async Task<BrowseLearnProgressViewModel> Get(Guid id)
    {
        var userId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();
        var learnProgress = await _repository.GetLearnProgress(id, userId);
        if (learnProgress != null)
            return learnProgress.MapToDto<BrowseLearnProgressViewModel>();
        else
            return new BrowseLearnProgressViewModel();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpPost]
    public async Task<LearnProgressQueryViewModel> GetList([FromBody] LearnProgressQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<LearnProgressQuery>();
        query.CreatorId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();
        await _repository.GetByQueryAsync(query);
        var result = query.MapToQueryDto<LearnProgressQueryViewModel, LearnProgress>();
        result.Result = result.Result.GroupBy(obj => obj.VideoId)
                 .Select(group => group.OrderByDescending(obj => obj.CreateTime).FirstOrDefault()).ToList();
        return result;
    }


    /// <summary>
    /// 听课记录按日期排序
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<UserProgressRecordByDateModel>> GetListByDate([FromBody] QueryByVideoIds model)
    {
        Guid CreatorId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();
        return await _repository.GetUserProgressRecordByDateList(model.VideoIds, CreatorId);
    }
    /// <summary>
    /// 读取用户的听课数据统计(左侧)
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<LearnRcordStaticViewModel> GetLearnRcordStaticAsync([FromBody] QueryByVideoIds model)
    {
        Guid CreatorId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();
        var Result = await GetList(new LearnProgressQueryViewModel { VideoIds = model.VideoIds, CreatorId = CreatorId, PageIndex = 1, PageSize = 9999 });
        var ReportModel = new LearnRcordStaticViewModel
        {
            LearnTotalTime = Math.Round((double)Result.Result.Sum(x => x.MaxProgress) / 3600, 2),
            VideoTotalTime = Math.Round((double)Result.Result.Sum(x => x.TotalProgress) / 3600, 2)
        };
        return ReportModel;
    }

    /// <summary>
    /// 读取用户的听课数据统计(右侧)
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<UserProgressByDateModel>> GetLearnRcordByWeekStaticAsync([FromBody]QueryLearnRcordByWeekStatic model)
    {
        Guid CreatorId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();
        var ReportData = await _repository.GetUserProgressByDateList(model.VideoIds, CreatorId, model.StartTime.ToString("yyyy-MM-dd"), model.EndTime.ToString("yyyy-MM-dd"));
        return ReportData.ToList();
    }
    /// <summary>
    /// 创建(完善这里的逻辑)
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public async Task Create([FromBody] CreateLearnProgressViewModel model)
    {
        var command = new CreateLearnProgressCommand(
            model.ProductId,
            model.SubjectId,
        model.ChapterId,
        model.CourseId,
        Guid.Parse(model.VideoId),
        model.Identifier,
        //用户学习总时长
        model.Progress,
        //视频总时长
        model.TotalProgress,
        //当前用户学习的最大学习进度
        model.MaxProgress
    );
        await _bus.SendCommand(command);
        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

    }
    /// <summary>
    /// 视频是否学完(更新)
    /// </summary>
    /// <param name="Input"></param>
    /// <returns></returns>
    /// <exception cref="GirvsException"></exception>
    [HttpPost]
    public async Task UpdateIsEnd(UpdateIsEndModel Input)
    {
        var command = new UpdateIsEndCommand(Input.VideoId);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }


    #endregion
}