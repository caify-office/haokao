using HaoKao.Common.RemoteModel;

namespace HaoKao.LearningPlanService.Domain.Records;

public record StateTask
{
    /// <summary>
    /// 智辅产品权限内容
    /// </summary>
    public ICollection<AssistantProductPermissionContent> AssistantProductPermissionContents { get; set; } = [];
}