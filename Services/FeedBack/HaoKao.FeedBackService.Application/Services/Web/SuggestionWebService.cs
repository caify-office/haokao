using HaoKao.FeedBackService.Application.Services.Management;
using HaoKao.FeedBackService.Application.ViewModels.Suggestion;

namespace HaoKao.FeedBackService.Application.Services.Web;

/// <summary>
/// 意见反馈接口服务 - Web
/// </summary>
/// <param name="service"></param>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class SuggestionWebService(ISuggestionService service) : ISuggestionWebService
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    public Task<BrowseSuggestionViewModel> Get(Guid id)
    {
        return service.Get(id);
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<QuerySuggestionViewModel> Get(int pageIndex, int pageSize)
    {
        return service.Get(new QuerySuggestionViewModel
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            CreatorId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>()
        });
    }

    /// <summary>
    /// 创建意见反馈
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public Task Create([FromBody] CreateSuggestionViewModel model)
    {
        return service.Create(model);
    }
}