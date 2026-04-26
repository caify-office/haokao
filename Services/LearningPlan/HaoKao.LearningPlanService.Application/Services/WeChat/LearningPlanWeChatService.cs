using HaoKao.Common;
using HaoKao.LearningPlanService.Application.Services.Web;
using HaoKao.LearningPlanService.Application.ViewModels.LearningPlan;

namespace HaoKao.LearningPlanService.Application.Services.WeChat;

/// <summary>
/// 学习计划主类，用于组织和管理一系列学习任务接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class LearningPlanWeChatService(ILearningPlanWebService service) : ILearningPlanWeChatService
{
    /// <summary>
    /// 获取当前用户学习计划
    /// </summary>
    [HttpGet("{productId:guid}/{subjectId:guid}")]
    public Task<BrowseLearningPlanViewModel> GetCurrentLearningPlan(Guid productId, Guid subjectId)
    {
        return service.GetCurrentLearningPlan(productId, subjectId);
    }

    /// <summary>
    /// 创建学习计划主类，用于组织和管理一系列学习任务
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public Task Create([FromBody] CreateLearningPlanViewModel model)
    {
        return service.Create(model);
    }

    /// <summary>
    /// 根据主键删除指定学习计划主类，用于组织和管理一系列学习任务
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id:guid}")]
    public Task Delete(Guid id)
    {
        return service.Delete(id);
    }

    /// <summary>
    /// 获取任务总数和总时长
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost("QueryTaskCountAndDurationsViewModel")]
    public Task<StatisticsTaskCountDurationsViewModel> Get([FromBody] QueryTaskCountAndDurationsViewModel model)
    {
        return service.Get(model);
    }
}