
using HaoKao.AnsweringQuestionService.Application.ViewModels;
using HaoKao.AnsweringQuestionService.Application.ViewModels.AnsweringQuestion;
using HaoKao.AnsweringQuestionService.Domain.Commands.AnsweringQuestion;
using HaoKao.AnsweringQuestionService.Domain.Entities;
using HaoKao.AnsweringQuestionService.Domain.Queries.EntityQuery;
using HaoKao.AnsweringQuestionService.Domain.Repositories;
using HaoKao.Common;


namespace HaoKao.AnsweringQuestionService.Application.AppService.Web;

/// <summary>
/// 答疑接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class AnsweringQuestionWebService(
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IAnsweringQuestionRepository repository
) : IAnsweringQuestionWebService
{
    #region 初始参数

    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IAnsweringQuestionRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 答题详情
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet]
    public async Task<BrowseAnsweringQuestionViewModel> Get(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id) ?? throw new GirvsException("对应的答疑不存在", StatusCodes.Status404NotFound);
        return entity.MapToDto<BrowseAnsweringQuestionViewModel>();
    }

    /// <summary>
    ///答疑列表（我的答疑和热门答疑）
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public async Task<AnsweringQuestionQueryWebViewModel> GetList([FromQuery] AnsweringQuestionQueryWebViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<AnsweringQuestionWebQuery>();
        await _repository.GetByQueryAsync(query);
        return query.MapToQueryDto<AnsweringQuestionQueryWebViewModel, AnsweringQuestion>();
    }

    /// <summary>
    /// 读取用户在某个产品下的提问数量
    /// </summary>
    /// <param name="ProductId"></param>
    /// <param name="UserId"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<int> GetUserCount([FromQuery] Guid ProductId, Guid UserId)
    {
        return _repository.GetCountByProductId(ProductId, UserId);
    }

    /// <summary>
    /// 创建答疑
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public async Task Create([FromBody] CreateAnsweringQuestionViewModel model)
    {
        var command = new CreateAnsweringQuestionCommand(
            model.ParentId,
            model.SubjectId,
            model.SubjectName,
            model.CourseId,
            model.CourseChapterId,
            model.CourseVideId,
            model.BookPageSize,
            model.BookName,
            model.Type,
            model.Description,
            model.Remark,
            model.FileUrl,
            model.CourseName,
            model.CourseChapterName,
            model.CourseVideoName,
            model.ProductId
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }
    /// <summary>
    /// 更新观看人数
    /// </summary>
    /// <param name="request">request</param>
    [HttpPost]
    public async Task UpdateWatchCount(UpdateWatchCountModel request)
    {
        var command = new UpdateAnsweringQuestionCommand(request.Id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}