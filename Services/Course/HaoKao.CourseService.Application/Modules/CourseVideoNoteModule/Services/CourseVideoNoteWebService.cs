using HaoKao.CourseService.Application.Modules.CourseVideoNoteModule.Interfaces;
using HaoKao.CourseService.Application.Modules.CourseVideoNoteModule.ViewModels;
using HaoKao.CourseService.Domain.CourseVideoNoteModule;

namespace HaoKao.CourseService.Application.Modules.CourseVideoNoteModule.Services;

/// <summary>
/// 课程视频笔记接口服务-web端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class CourseVideoNoteWebService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    ICourseVideoNoteRepository repository
) : ICourseVideoNoteWebService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly ICourseVideoNoteRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id}")]
    public async Task<BrowseCourseVideoNoteViewModel> Get(Guid id)
    {
        var courseVideoNote = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<CourseVideoNote>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的课程视频笔记不存在", StatusCodes.Status404NotFound);

        return courseVideoNote.MapToDto<BrowseCourseVideoNoteViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public async Task<QueryCourseVideoNoteViewModel> Get([FromQuery] QueryCourseVideoNoteViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<CourseVideoNoteQuery>();
        var cacheKey = GirvsEntityCacheDefaults<CourseVideoNote>.QueryCacheKey.Create(query.GetCacheKey());
        var tempQuery = await _cacheManager.GetAsync(cacheKey, async () =>
        {
            await _repository.GetByQueryAsync(query);
            return query;
        });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<QueryCourseVideoNoteViewModel, CourseVideoNote>();
    }

    /// <summary>
    /// 创建课程视频笔记
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public async Task Create([FromBody] CreateCourseVideoNoteViewModel model)
    {
        var command = model.MapToCommand<CreateCourseVideoNoteCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定课程视频笔记
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id}")]
    public async Task Delete(Guid id)
    {
        var command = new DeleteCourseVideoNoteCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据视频id删除对应的所有笔记
    /// </summary>
    /// <param name="videoId">视频id</param>
    [NonAction]
    public async Task DeleteBatch(string videoId)
    {
        var command = new DeleteBatchCourseVideoNoteCommand(videoId);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定课程视频笔记
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPut]
    public async Task Update([FromBody] UpdateCourseVideoNoteViewModel model)
    {
        var command = model.MapToCommand<UpdateCourseVideoNoteCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}