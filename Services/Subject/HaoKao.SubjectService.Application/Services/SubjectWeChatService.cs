using HaoKao.SubjectService.Application.Interfaces;
using HaoKao.SubjectService.Application.ViewModels;

namespace HaoKao.SubjectService.Application.Services;

/// <summary>
/// 科目接口服务---WeChat
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class SubjectWeChatService(ISubjectWebService service) : ISubjectWeChatService
{
    private readonly ISubjectWebService _service = service ?? throw new ArgumentNullException(nameof(service));

    /// <summary>
    /// 读取科目列表,不带分页
    /// </summary>
    [HttpGet, AllowAnonymous]
    public Task<IReadOnlyList<BrowseSubjectViewModel>> Get()
    {
        return _service.Get();
    }

    /// <summary>
    /// 按租户Id获取科目列表
    /// </summary>
    [HttpGet, AllowAnonymous]
    public Task<IReadOnlyList<BrowseSubjectViewModel>> GetListByTenantId(Guid tenantId)
    {
        return _service.GetListByTenantId(tenantId);
    }

    /// <summary>
    /// 获取公共科目列表
    /// </summary>
    [HttpGet]
    public Task<IReadOnlyList<BrowseSubjectViewModel>> GetCommonSubjectList()
    {
        return _service.GetCommonSubjectList();
    }
}