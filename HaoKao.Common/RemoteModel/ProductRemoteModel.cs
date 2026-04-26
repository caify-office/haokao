using System.ComponentModel;

namespace HaoKao.Common.RemoteModel;

public record AssistantProductPermissionModel(Guid ProductId, Guid SubjectId, ICollection<AssistantProductPermissionContent> AssistantProductPermissionContents);

public record AssistantProductPermissionContent(Guid PermissionId, string PermissionName, AssistanPermissionType AssistanPermissionType);


/// <summary>
/// 权限类别
/// </summary>
public enum AssistanPermissionType
{
    /// <summary>
    /// 阶段课程
    /// </summary>
    [Description("阶段课程")]
    Course = 0,

    /// <summary>
    /// 试卷
    /// </summary>
    [Description("试卷")]
    Paper = 1,

    /// <summary>
    /// 智辅课程
    /// </summary>
    [Description("智辅课程")]
    AssistanCourse = 2,
}