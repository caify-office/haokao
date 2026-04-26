using HaoKao.Common.Extensions;
using HaoKao.CourseService.Application.Modules.CourseVideoModule.Interfaces;
using HaoKao.CourseService.Application.Modules.CourseVideoModule.ViewModels;
using HaoKao.CourseService.Application.Modules.CourseVideoNoteModule.Interfaces;
using HaoKao.CourseService.Application.Modules.VideoStorageModule.Interfaces;
using HaoKao.CourseService.Domain.CourseChapterModule;
using HaoKao.CourseService.Domain.CourseMaterialsModule;
using HaoKao.CourseService.Domain.CoursePracticeModule;
using HaoKao.CourseService.Domain.CourseVideoModule;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HaoKao.CourseService.Application.Modules.CourseVideoModule.Services;

/// <summary>
/// 课程视频接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "课程视频",
    "d816135a-d0f6-ff2b-2b7c-57f88bc9efd8",
    "128",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class CourseVideoService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    ICourseVideoRepository repository,
    ICoursePracticeRepository practiceRepository,
    ICourseMaterialsRepository materialsRepository,
    ICourseChapterRepository courseChapterRepository,
    ICourseVideoNoteWebService courseVideoNoteWebService,
    IVideoStorageService videoStorageService,
    ILogger<CourseVideoService> logger
) : ICourseVideoService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly ICourseVideoRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ICoursePracticeRepository _practiceRepository = practiceRepository;
    private readonly ICourseMaterialsRepository _materialsRepository = materialsRepository;
    private readonly ICourseChapterRepository _courseChapterRepository = courseChapterRepository ?? throw new ArgumentNullException(nameof(courseChapterRepository));
    private readonly ICourseVideoNoteWebService _courseVideoNoteWebService = courseVideoNoteWebService ?? throw new ArgumentNullException(nameof(courseVideoNoteWebService));
    private readonly IVideoStorageService _videoStorageService = videoStorageService ?? throw new ArgumentNullException(nameof(videoStorageService));
    private readonly ILogger<CourseVideoService> logger = logger;

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseCourseVideoViewModel> Get(Guid id)
    {
        var courseVideo = await _repository.GetByIdAsync(id) ?? throw new GirvsException("对应的课程视频不存在", StatusCodes.Status404NotFound);
        return courseVideo.MapToDto<BrowseCourseVideoViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<QueryCourseVideoViewModel> Get([FromQuery] QueryCourseVideoViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<CourseVideoQuery>();
        await _repository.GetByQueryAsync(query);
        return query.MapToQueryDto<QueryCourseVideoViewModel, CourseVideo>();
    }

    /// <summary>
    /// 读取课程下面的更新资料接口
    /// </summary>
    /// <param name="courseIds"></param>
    /// <returns></returns>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<List<UpdateCourseVideoNewViewModel>> GetUpdateCourseVideoList([FromQuery] string courseIds)
    {
        var videos = await _repository.GetUpdateVideoList(courseIds);
        var result = MapUpdateVideoModel(videos);
        return result;
    }

    private static List<UpdateCourseVideoNewViewModel> MapUpdateVideoModel(List<UpdateVideoModel> videos)
    {
        return videos.OrderByDescending(x => x.createtime)
                     .Select(x => x.createtime).Distinct()
                     .Select(time =>
                     {
                         var courses = videos.Where(x => x.createtime == time)
                                             .Select(x => x.coursename).Distinct()
                                             .Select(coursename =>
                                             {
                                                 var videoNames = videos.Where(x => x.createtime == time & x.coursename == coursename)
                                                                        .Select(x => x.videoname).ToList();
                                                 return new CourseInfo(coursename, videoNames);
                                             }).ToList();
                         return new UpdateCourseVideoNewViewModel(time, courses);
                     }).ToList();
    }

    /// <summary>
    /// 读取课程下面的更新资料接口(智辅课程专用)
    /// </summary>
    /// <param name="courseIds"></param>
    /// <returns></returns>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<List<UpdateCourseVideoNewViewModel>> GetUpdateAssistantCourseVideoList([FromQuery] string courseIds)
    {
        var videos = await _repository.GetUpdateAssistantVideoList(courseIds);
        var result = MapUpdateVideoModel(videos);
        return result;
    }

    /// <summary>
    /// 根据课程ids拿到多个课程下面所有的
    /// </summary>
    /// <param name="courseIds"></param>
    /// <returns></returns>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<List<CourseVideoQueryListViewModel>> GetVideoIdsByCourseIds([FromQuery] string courseIds)
    {
        var chapters = await _courseChapterRepository.GetWhereAsync(predicate => courseIds.Contains(predicate.CourseId.ToString()));
        var ids = chapters.Select(x => x.Id);
        var result = await _repository.GetWhereAsync(x => ids.Contains(x.CourseChapterId) || courseIds.Contains(x.CourseChapterId.ToString()));
        return result.MapTo<List<CourseVideoQueryListViewModel>>();
    }

    /// <summary>
    /// 补充智辅课程知识点信息
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<QueryKnowledgePointConfigInfoViewModel> QueryKnowledgePointConfigInfo([FromBody] QueryKnowledgePointConfigInfoViewModel model)
    {
        var knowledgePointIds = model.Result.Select(x => x.KnowledgePointId).ToList();
        await FillCourseVideoInfo(model, knowledgePointIds);
        await FillCoursePracticeInfo(model, knowledgePointIds);
        await FillCourseMaterialsInfo(model, knowledgePointIds);

        return model;

        async Task FillCourseVideoInfo(QueryKnowledgePointConfigInfoViewModel model, List<Guid> knowledgePointIds)
        {
            var courseVideos = await _repository.GetWhereAsync(predicate => predicate.CourseChapterId == model.CourseChapterId
                                                                         && knowledgePointIds.Contains(predicate.KnowledgePointId));
            var courseVideoDic = courseVideos.GroupBy(x => x.KnowledgePointId).ToDictionary(x => x.Key, x => x.Last());
            foreach (var item in model.Result)
            {
                if (courseVideoDic.ContainsKey(item.KnowledgePointId))
                {
                    item.Id = courseVideoDic[item.KnowledgePointId].Id;
                    item.VideoName = courseVideoDic[item.KnowledgePointId].VideoName;
                    item.Duration = courseVideoDic[item.KnowledgePointId].Duration;
                }
                else
                {
                    item.Id = Guid.Empty;
                    item.VideoName = string.Empty;
                    item.Duration = 0;
                }
            }
        }

        async Task FillCoursePracticeInfo(QueryKnowledgePointConfigInfoViewModel model, List<Guid> knowledgePointIds)
        {
            var CoursePractices = await _practiceRepository.GetWhereAsync(predicate => predicate.CourseId == model.CourseChapterId
                                                                                    && knowledgePointIds.Contains(predicate.KnowledgePointId));
            var CoursePracticeDic = CoursePractices.GroupBy(x => x.KnowledgePointId).ToDictionary(x => x.Key, x => x.Last());
            foreach (var item in model.Result)
            {
                if (CoursePracticeDic.ContainsKey(item.KnowledgePointId))
                {
                    item.QuestionConfig = CoursePracticeDic[item.KnowledgePointId].QuestionConfig;
                    item.QuestionCount = CoursePracticeDic[item.KnowledgePointId].QuestionCount;
                }
                else
                {
                    item.Id = Guid.Empty;
                    item.QuestionConfig = string.Empty;
                    item.QuestionCount = 0;
                }
            }
        }

        async Task FillCourseMaterialsInfo(QueryKnowledgePointConfigInfoViewModel model, List<Guid> knowledgePointIds)
        {
            var CourseMaterialss = await _materialsRepository.GetWhereAsync(predicate => predicate.CourseChapterId == model.CourseChapterId
                                                                                      && knowledgePointIds.Contains(predicate.KnowledgePointId));
            var CourseMaterialsDic = CourseMaterialss.GroupBy(x => x.KnowledgePointId).ToDictionary(x => x.Key, x => x.Last());
            foreach (var item in model.Result)
            {
                if (CourseMaterialsDic.ContainsKey(item.KnowledgePointId))
                {
                    item.FileUrl = CourseMaterialsDic[item.KnowledgePointId].FileUrl;
                    item.Name = CourseMaterialsDic[item.KnowledgePointId].Name;
                }
                else
                {
                    item.FileUrl = string.Empty;
                    item.Name = string.Empty;
                }
            }
        }
    }

    /// <summary>
    /// 保存课程视频（智辅课程专用）
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("创建", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Save([FromBody] SaveCourseVideoViewModel model)
    {
        var command = model.MapToCommand<SaveCourseVideoCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 批量添加视频
    /// </summary>
    /// <param name="models"></param>
    /// <returns></returns>
    /// <exception cref="GirvsException"></exception>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("创建", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task BatchCreate([FromBody] List<SaveCourseVideoViewModel> models)
    {
        var command = new CreateCourseVideoBatchCommand(
            models.MapTo<List<CourseVideo>>()
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定课程视频
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var video = await Get(id);
        var videoId = video.VideoId;
        var command = new DeleteCourseVideoCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
        await _courseVideoNoteWebService.DeleteBatch(videoId);
    }

    /// <summary>
    /// 根据主键更新指定课程视频
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    [HttpPut("{id}")]
    [ServiceMethodPermissionDescriptor("更新", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update(Guid id, [FromBody] UpdateCourseVideoViewModel model)
    {
        var video = await Get(id);
        var videoId = video.VideoId;
        var command = model.MapToCommand<UpdateCourseVideoCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
        if (videoId != model.VideoId)
        {
            //视频id被替换，之前视频id关联的视频笔记全部删除
            await _courseVideoNoteWebService.DeleteBatch(videoId);
        }
    }

    /// <summary>
    /// 设置知识点(单个)
    /// </summary>
    /// <param name="rquest"></param>
    /// <returns></returns>
    /// <exception cref="GirvsException"></exception>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("更新", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task UpdateKnowledgePoint(UpdateKnowledgePointModel rquest)
    {
        var command = new UpdateKnowledgePointCommand(rquest.Id, rquest.KnowledgepointIds);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 设置排序(单个)
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="GirvsException"></exception>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("更新", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task UpdateSort(UpdateSortModel request)
    {
        var command = new UpdateSortCommand(request.Id, request.Sort);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 设置试听(支持批量)
    /// </summary>
    [HttpPatch]
    [ServiceMethodPermissionDescriptor("试听", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public Task SetTry([FromBody] IEnumerable<Guid> ids)
    {
        return UpdateIsTry(ids, true);
    }

    /// <summary>
    ///取消试听(支持批量)
    /// </summary>
    [HttpPatch]
    [ServiceMethodPermissionDescriptor("取消试听", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public Task CancelTry([FromBody] IEnumerable<Guid> ids)
    {
        return UpdateIsTry(ids, false);
    }

    /// <summary>
    /// 批量修改前缀(支持批量)
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="GirvsException"></exception>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("批量修改前缀", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task BatchUpdateName(BatchUpdateNameViewModel request)
    {
        var command = new BatchUpdateNameCommand(request.Ids, request.Name);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 设置取消试听
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    /// <exception cref="GirvsException"></exception>
    private async Task UpdateIsTry(IEnumerable<Guid> ids, bool state)
    {
        var command = new UpdateIsTryCommand(ids, state);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion

    #region 更新所有租户下视频信息

    /// <summary>
    /// 更新所有租户下视频标签和视频分类
    /// </summary>
    [HttpPut]
    [AllowAnonymous]
    public async Task UpdateVideoInfo([FromServices] IServiceProvider provider)
    {
        await _repository.AutoMigrationAllTenantId();
        var tableNames = await _repository.GetTableNames();
        foreach (var table in tableNames)
        {
            table.SetTenantId();
            await using var scope = provider.CreateAsyncScope();
            var repo = scope.ServiceProvider.GetRequiredService<ICourseVideoRepository>();

            var list = await repo.GetAllAsync();
            if (list.Count == 0) continue;

            logger.LogInformation($"当前租户{table}需要同步总数：{list.Count}");

            int completeNumber = 0;
            foreach (var course in list)
            {
                var videoInfo = await _videoStorageService.GetVideoInfo(course.VideoId);
                course.VideoName = videoInfo?.Title ?? course.VideoName;

                logger.LogInformation($"当前租户{table}已完成{++completeNumber}/{list.Count}");
            }
            await repo.UpdateRangeAsync(list);
        }
    }

    #endregion
}