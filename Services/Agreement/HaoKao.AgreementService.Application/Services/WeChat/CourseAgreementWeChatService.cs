using HaoKao.AgreementService.Application.Services.Web;
using HaoKao.AgreementService.Application.ViewModels.CourseAgreement;
using HaoKao.AgreementService.Domain.Entities;
using HaoKao.AgreementService.Domain.Queries;
using HaoKao.AgreementService.Domain.Repositories;

namespace HaoKao.AgreementService.Application.Services.WeChat;

/// <summary>
/// 课程协议接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class CourseAgreementWeChatService(
    ICourseAgreementWebService service,
    ICourseAgreementRepository repository,
    IStaticCacheManager cacheManager,
    IMapper mapper
) : ICourseAgreementWeChatService
{
    #region 初始参数

    private readonly ICourseAgreementWebService _service = service ?? throw new ArgumentNullException(nameof(service));

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
    /// 根据ids获取列表(包含公共协议)
    /// </summary>
    /// <param name="ids">查询对象</param>
    [HttpPost]
    public async Task<List<BrowseCourseAgreementViewModel>> GetListWithCommonAgreement([FromBody] IReadOnlyList<Guid> ids)
    {
        var query = new CourseAgreementQuery { AgreementType = AgreementType.Standard };
        var key = JsonSerializer.Serialize(query).ToMd5();
        var list = await cacheManager.GetAsync(
            GirvsEntityCacheDefaults<CourseAgreement>.QueryCacheKey.Create(key),
            () => repository.GetWhereAsync(query.GetQueryWhere())
        );
        var result = mapper.Map<List<BrowseCourseAgreementViewModel>>(list);
        result.AddRange(await _service.Get(ids));
        return result;
    }

    /// <summary>
    /// 获取公共协议Id和名称
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    [HttpGet]
    public Task<Dictionary<Guid, string>> GetWithCommonAgreement(Guid? id)
    {
        return _service.GetWithCommonAgreement(id);
    }

    #endregion
}