using HaoKao.CourseService.Application.Modules.CourseMaterialsModule.Interfaces;
using HaoKao.CourseService.Application.Modules.CourseMaterialsModule.ViewModels;
using HaoKao.CourseService.Domain.CourseChapterModule;
using HaoKao.CourseService.Domain.CourseMaterialsModule;

namespace HaoKao.CourseService.Application.Modules.CourseMaterialsModule.Services;

/// <summary>
/// 课程讲义接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class CourseMaterialsWebService(ICourseMaterialsRepository repository, ICourseChapterRepository courseChapterRepository) : ICourseMaterialsWebService
{
    private readonly ICourseMaterialsRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ICourseChapterRepository _courseChapterRepository = courseChapterRepository ?? throw new ArgumentNullException(nameof(courseChapterRepository));

    /// <summary>
    /// 根据查询获取列表，用于分页(读取课程下所有的讲义)
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public async Task<QueryCourseMaterialsViewModel> Get([FromQuery] QueryCourseMaterialsViewModel queryViewModel)
    {
        var coursechapters = await _courseChapterRepository.GetWhereAsync(predicate => predicate.CourseId == queryViewModel.CourseId);
        var query = queryViewModel.MapToQuery<CourseMaterialsQuery>();
        await _repository.GetByQueryAsync(query);
        query.CourseChapterIds = coursechapters.Select(x => x.Id).ToList();
        await _repository.GetByQueryAsync(query);
        return query.MapToQueryDto<QueryCourseMaterialsViewModel, CourseMaterials>();
    }

    /// <summary>
    ///下载讲义（阶段学习传课程章节Id，智辅课程学习传课程Id）
    /// </summary>
    /// <param name="CourseId">（阶段学习传课程章节Id，智辅课程学习传课程Id）</param>
    /// <returns></returns>
    [HttpGet]
    public Task<List<CourseMaterials>> DownLoadMaterials([FromQuery] Guid CourseId)
    {
        return _repository.GetByQueryByChapterIdsAsync(CourseId);
    }
}