using HaoKao.Common;
using HaoKao.LearningPlanService.Application.Services.Web;
using HaoKao.LearningPlanService.Application.ViewModels.LearningTask;

namespace HaoKao.LearningPlanService.Application.Services.WeChat;

/// <summary>
/// 学习任务主类接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class LearningTaskWeChatService(ILearningTaskWebService service) : ILearningTaskWeChatService
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    public Task<BrowseLearningTaskViewModel> Get(Guid id)
    {
        return service.Get(id);
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public Task<LearningTaskQueryViewModel> Get([FromQuery] LearningTaskQueryViewModel queryViewModel)
    {
        return service.Get(queryViewModel);
    }
}