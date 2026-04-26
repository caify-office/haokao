using HaoKao.CourseFeatureService.Application.Interfaces;
using HaoKao.CourseFeatureService.Application.ViewModels;
using HaoKao.CourseFeatureService.Domain;

namespace HaoKao.CourseFeatureService.Application.Services;

/// <summary>
/// 课程特色服务接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class CourseFeatureWebService(ICourseFeatureRepository repository, IStaticCacheManager cache, IMapper mapper) : ICourseFeatureWebService
{
    private readonly ICourseFeatureRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IStaticCacheManager _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    /// <summary>
    /// 根据ids获取列表
    /// </summary>
    /// <param name="ids"></param>
    [HttpPost, AllowAnonymous]
    public async Task<List<BrowseCourseFeatureWebViewModel>> Get([FromBody] List<Guid> ids)
    {
        var list = await _cache.GetAsync(
            GirvsEntityCacheDefaults<CourseFeature>.ByIdsCacheKey.Create(string.Join(',', ids).ToMd5()),
            () => _repository.GetWhereAsync(x => ids.Contains(x.Id))
        );
        return _mapper.Map<List<BrowseCourseFeatureWebViewModel>>(list.Where(x => x.Enable));
    }
}