using HaoKao.CourseRatingService.Application.Services.Management;
using HaoKao.CourseRatingService.Application.ViewModels;
using HaoKao.CourseRatingService.Domain.Entities;
using HaoKao.CourseRatingService.Domain.Enums;
using HaoKao.CourseRatingService.Domain.Queries;
using HaoKao.CourseRatingService.Domain.Repositories;

namespace HaoKao.CourseRatingService.Application.Services.Web;

/// <summary>
/// 课程评价接口服务PC端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class CourseRatingWebService(
    IStaticCacheManager cacheManager,
    ICourseRatingRepository repository,
    ICourseRatingService service
) : ICourseRatingWebService
{
    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet, AllowAnonymous]
    public async Task<QueryCourseRatingWebViewModel> Get([FromQuery] QueryCourseRatingWebViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<CourseRatingQuery>();
        query.AuditState = AuditState.Pass;
        var cacheKey = GirvsEntityCacheDefaults<CourseRating>.QueryCacheKey.Create(query.GetCacheKey());
        var tempQuery = await cacheManager.GetAsync(cacheKey, async () =>
        {
            await repository.GetCourseRatingForWeb(query);
            return query;
        });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<QueryCourseRatingWebViewModel, CourseRating>();
    }

    /// <summary>
    /// 创建课程评价
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public Task Create([FromBody] CreateCourseRatingViewModel model)
    {
        return service.Create(model);
    }
}