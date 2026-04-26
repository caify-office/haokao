using HaoKao.StudyMaterialService.Application.Interfaces;
using HaoKao.StudyMaterialService.Domain.Entities;
using HaoKao.StudyMaterialService.Domain.Repositories;

namespace HaoKao.StudyMaterialService.Application.Services;

/// <summary>
/// 学习资料接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class StudyMaterialWebService(
    IStaticCacheManager cacheManager,
    IStudyMaterialRepository repository
) : IStudyMaterialWebService
{
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IStudyMaterialRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    /// <summary>
    /// 根据ids获取资料列表
    /// </summary>
    /// <param name="ids">ids</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<Material>> Get([FromBody] List<Guid> ids)
    {
        var list = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<StudyMaterial>.ByIdsCacheKey.Create(string.Join(',', ids).ToMd5()),
            () => _repository.GetWhereAsync(x => ids.Contains(x.Id))
        );
        return list.Where(x => x.Enable).SelectMany(x => x.Materials).ToList();
    }
}