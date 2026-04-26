using HaoKao.AgreementService.Application.Services.Management;
using HaoKao.AgreementService.Application.ViewModels.CourseAgreement;
using HaoKao.AgreementService.Domain.Entities;
using HaoKao.AgreementService.Domain.Queries;
using HaoKao.AgreementService.Domain.Repositories;

namespace HaoKao.AgreementService.Application.Services.Web;

/// <summary>
/// 课程协议接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class CourseAgreementWebService(
    ICourseAgreementService service,
    ICourseAgreementRepository repository,
    IStaticCacheManager cacheManager
) : ICourseAgreementWebService
{
    #region 初始参数

    private readonly ICourseAgreementService _service = service ?? throw new ArgumentNullException(nameof(service));
    private readonly ICourseAgreementRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    public Task<BrowseCourseAgreementViewModel> Get(Guid id)
    {
        return _service.Get(id);
    }

    /// <summary>
    /// 根据ids获取列表
    /// </summary>
    /// <param name="ids">查询对象</param>
    [HttpPost]
    public Task<List<BrowseCourseAgreementViewModel>> Get([FromBody] IReadOnlyList<Guid> ids)
    {
        return _service.GetByIds(ids);
    }

    /// <summary>
    /// 获取公共协议Id和名称
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<Dictionary<Guid, string>> GetWithCommonAgreement(Guid? id)
    {
        var query = new CourseAgreementQuery { AgreementType = AgreementType.Standard };
        var key = JsonSerializer.Serialize(query).ToMd5();
        var list = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<CourseAgreement>.QueryCacheKey.Create(key),
            () => _repository.GetWhereAsync(query.GetQueryWhere())
        );

        var dict = list.ToDictionary(k => k.Id, v => v.Name);

        if (id.HasValue)
        {
            var agreement = await _service.Get(id.Value);
            dict.TryAdd(agreement.Id, agreement.Name);
        }

        return dict;
    }

    #endregion
}