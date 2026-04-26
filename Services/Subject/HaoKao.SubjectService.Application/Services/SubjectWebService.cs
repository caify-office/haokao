using HaoKao.SubjectService.Application.Interfaces;
using HaoKao.SubjectService.Application.ViewModels;
using HaoKao.SubjectService.Domain.SubjectModule;

namespace HaoKao.SubjectService.Application.Services;

/// <summary>
/// 科目接口服务---web
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class SubjectWebService(IStaticCacheManager cacheManager, ISubjectRepository repository) : ISubjectWebService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly ISubjectRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 读取科目列表,不带分页
    /// </summary>
    [HttpGet, AllowAnonymous]
    public async Task<IReadOnlyList<BrowseSubjectViewModel>> Get()
    {
        var key = GirvsEntityCacheDefaults<Subject>.QueryCacheKey.Create("All");
        var result = await _cacheManager.GetAsync(key, async () =>
        {
            var ts = await _repository.GetAllAsync();
            return ts.Where(x => x.IsShow).ToList();
        });
        return result.MapTo<List<BrowseSubjectViewModel>>();
    }

    /// <summary>
    /// 按租户Id获取科目列表
    /// </summary>
    [HttpGet]
    public async Task<IReadOnlyList<BrowseSubjectViewModel>> GetListByTenantId(Guid tenantId)
    {
        var keyStr = $"{nameof(Subject).ToLowerInvariant()}:TenantId_{tenantId}:list:all:query";
        var key = GirvsEntityCacheDefaults<Subject>.BuideCustomize(keyStr).Create();
        var result = await _cacheManager.GetAsync(key, async () =>
        {
            var ts = await _repository.GetWhereAsync(x => x.TenantId == tenantId);
            return ts.Where(x => x.IsShow).ToList();
        });
        return result.MapTo<List<BrowseSubjectViewModel>>();
    }

    /// <summary>
    /// 获取公共科目列表
    /// </summary>
    [HttpGet]
    public async Task<IReadOnlyList<BrowseSubjectViewModel>> GetCommonSubjectList()
    {
        var subjectType = SubjectTypeEnum.Common;
        var key = GirvsEntityCacheDefaults<Subject>.QueryCacheKey.Create($"IsCommon={subjectType}");
        var result = await _cacheManager.GetAsync(key, async () =>
        {
            var ts = await _repository.GetWhereAsync(x => x.IsCommon == subjectType);
            return ts.Where(x => x.IsShow).ToList();
        });
        return result.MapTo<List<BrowseSubjectViewModel>>();
    }

    #endregion
}