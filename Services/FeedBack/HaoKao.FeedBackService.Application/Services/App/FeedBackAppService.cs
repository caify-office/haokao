using HaoKao.FeedBackService.Application.Services.Management;
using HaoKao.FeedBackService.Application.ViewModels.FeedBack;

namespace HaoKao.FeedBackService.Application.Services.App;

/// <summary>
/// 接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.App)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class FeedBackAppService(
    IFeedBackService feedBackService
) : IFeedBackAppService
{
    #region 初始参数

    private readonly IFeedBackService _feedBackService = feedBackService ?? throw new ArgumentNullException(nameof(feedBackService));

    #endregion

    #region 服务方法

    /// <summary>
    /// 读取当前用户的提交次数
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<int> GetUserCount()
    {
        return await _feedBackService.GetUserCount();
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public async Task Create([FromBody] CreateFeedBackViewModel model)
    {
        await _feedBackService.Create(model);
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public async Task<FeedBackQueryViewModel> Get([FromQuery] FeedBackQueryViewModel queryViewModel)
    {
        return await _feedBackService.Get(queryViewModel);
    }

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="Id">主键</param>
    [HttpGet("{id}")]
    public async Task<BrowseFeedBackViewModel> Get(Guid Id)
    {
        return await _feedBackService.Get(Id);
    }

    #endregion
}