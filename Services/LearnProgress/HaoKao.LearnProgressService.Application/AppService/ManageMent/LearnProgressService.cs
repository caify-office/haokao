using Girvs.AuthorizePermission.Enumerations;
using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.LearnProgressService.Application.ViewModels.LearnProgress;
using HaoKao.LearnProgressService.Domain.Commands.LearnProgress;
using HaoKao.LearnProgressService.Domain.Entities;
using HaoKao.LearnProgressService.Domain.Queries.EntityQuery;
using HaoKao.LearnProgressService.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace HaoKao.LearnProgressService.Application.AppService.ManageMent;

/// <summary>
/// 接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "进度服务",
    "08db4f6b-b675-4c07-8391-fa04b29e2689",
    "1",
       SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class LearnProgressService(
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    ILearnProgressRepository repository
) : ILearnProgressService
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
    /// <param name="userId"></param>
    [HttpGet("{productId}/{subjectId}/{userId}")]
    public async  Task<float> GetLearnDurations( Guid productId,Guid subjectId ,Guid userId )
    {
        var result= await _repository.GetLearnDurations(productId, subjectId, userId);
        //转为小时
        return (float)Math.Round(result / 3600, 2);
    }
    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpPost]
    public async Task<LearnProgressQueryViewModel> GetList([FromBody] LearnProgressQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<LearnProgressQuery>();
        //query.CreatorId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();
        await _repository.GetByQueryAsync(query);
        var result = query.MapToQueryDto<LearnProgressQueryViewModel, LearnProgress>();
        result.Result = result.Result.GroupBy(obj => obj.VideoId)
                 .Select(group => group.OrderByDescending(obj => obj.CreateTime).FirstOrDefault()).ToList();
        return result;
    }

    /// <summary>
    /// 查询单一租户下特定用户特定产品特定科目下特定章节的听课进度数据
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpPost]
    public async Task<QueryCourseLearningProgressViewModel> QueryCourseLearningProgress([FromBody] QueryCourseLearningProgressViewModel queryViewModel)
    {
        var dataTable= await  _repository.QueryCourseLearningProgress(queryViewModel.StudentId, queryViewModel.ProductId, queryViewModel.SubjectId,queryViewModel.PageIndex,queryViewModel.PageSize);
        queryViewModel.RecordCount = dataTable.Item1;
        queryViewModel.Result = dataTable.Item2.Select(x=>
        new QueryCourseLearningProgressListViewModel {
            PermissionName=x["PermissionName"].ToString(),
            CourseChapterId =Guid.Parse(x["CourseChapterId"].ToString()),
            CourseChapterName=x["CourseChapterName"].ToString(),
            Duration =float.Parse(x["Duration"].ToString()),
            MaxProgress = float.Parse(x["MaxProgress"].ToString()),
            CreateTime = DateTime.Parse(x["CreateTime"].ToString()),
        }).ToList();
        return queryViewModel;
    }

    /// <summary>
    /// 统计学员学习进度
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpPost]
    public async Task<CourseLearningProgressStatisticsViewModel> CourseLearningProgressStatistics([FromBody] CourseLearningProgressStatisticsViewModel queryViewModel)
    {
        var dataTable = await _repository.CourseLearningProgressStatistics(queryViewModel.StudentId, queryViewModel.ProductId, queryViewModel.SubjectId);
        queryViewModel.CouseCount = dataTable.Item1;
        queryViewModel.CouseIsEndCount = dataTable.Item2;
        queryViewModel.Result = dataTable.Item3.Select(x =>
        new CouseCategoryStaticsViewModel
        {
            Category = Guid.Parse(x["Category"].ToString()),
            Duration = float.Parse(x["Duration"].ToString()),
            MaxProgress = float.Parse(x["MaxProgress"].ToString()),
        }).ToList();
        return queryViewModel;
    }

    /// <summary>
    /// 批量更新替换视频的学习进度
    /// </summary>
    /// <param name="Input"></param>
    /// <returns></returns>
    /// <exception cref="GirvsException"></exception>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("更新", Permission.Edit,
                                       UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task UpdateThVideoProgress(UpdateThVideoProgressModel Input)
    {
        var command = new UpdateThVideoProgressCommand(Input.VideoId,Input.Duration);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }
    /// <summary>
    /// 合并学习进度表
    /// </summary>
    /// <returns></returns>
    [HttpPut,AllowAnonymous]
    public async Task MergeLearnProgress()
    {
        await _repository.MergeLearnProgressDo();
    }
    #endregion

}