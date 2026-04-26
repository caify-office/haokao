
using HaoKao.AnsweringQuestionService.Application.AppService.Web;
using HaoKao.AnsweringQuestionService.Application.ViewModels;
using HaoKao.AnsweringQuestionService.Application.ViewModels.AnsweringQuestion;
using HaoKao.Common;

namespace HaoKao.AnsweringQuestionService.Application.AppService.WeChat;

/// <summary>
/// 答疑接口服务--wehcat
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class AnsweringQuestionWeChatService(
   IAnsweringQuestionWebService webService
) : IAnsweringQuestionWeChatService
{
    #region 初始参数
    private readonly IAnsweringQuestionWebService _webService = webService ?? throw new ArgumentNullException(nameof(webService));

    #endregion

    #region 服务方法

    /// <summary>
    /// 答题详情
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet]
    public Task<BrowseAnsweringQuestionViewModel> Get(Guid id)
    {
        return _webService.Get(id);
    }

    /// <summary>
    ///答疑列表（我的答疑和热门答疑）
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public Task<AnsweringQuestionQueryWebViewModel> GetList([FromQuery] AnsweringQuestionQueryWebViewModel queryViewModel)
    {
        return _webService.GetList(queryViewModel);
    }

    /// <summary>
    /// 读取用户在某个产品下的提问数量
    /// </summary>
    /// <param name="ProductId"></param>
    /// <param name="UserId"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<int> GetUserCount([FromQuery] Guid ProductId, Guid UserId)
    {
        return _webService.GetUserCount(ProductId, UserId);
    }

    /// <summary>
    /// 创建答疑
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public Task Create([FromBody] CreateAnsweringQuestionViewModel model)
    {
        return _webService.Create(model);
    }
    /// <summary>
    /// 更新观看人数
    /// </summary>
    /// <param name="request">request</param>
    [HttpPost]
    public Task UpdateWatchCount(UpdateWatchCountModel request)
    {
        return _webService.UpdateWatchCount(request);
    }

    #endregion
}