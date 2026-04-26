namespace HaoKao.ProductService.Domain.Commands.ProductPermission;

/// <summary>
/// 产品权限列表
/// </summary>
/// <param name="SubjectId">科目ID</param>
/// <param name="SubjectName">科目名称</param>
/// <param name="PermissionName">权限名称</param>
/// <param name="PermissionId">权限ID</param>
/// <param name="Category">分类名称</param>
public record ProductPermissionCommand(
    Guid SubjectId,
    string SubjectName,
    string PermissionName,
    Guid PermissionId,
    string Category
) : Command("产品权限列表");

/// <summary>
/// 智辅产品权限列表
/// </summary>
/// <param name="SubjectId">科目ID</param>
/// <param name="SubjectName">科目名称</param>
/// <param name="CourseStageId">阶段id</param>
/// <param name="CourseStageName">阶段名称</param>
/// <param name="StartTime">开始时间</param>
/// <param name="EndTime">结束时间</param>
/// <param name="AssistantProductPermissionContents">智辅课程列表</param>
public record AssistantProductPermissionCommand(
    Guid SubjectId,
    string SubjectName,
    Guid CourseStageId,
    string CourseStageName,
    DateTime StartTime,
    DateTime EndTime,
    ICollection<AssistantProductPermissionContent> AssistantProductPermissionContents
) : Command("智辅产品权限列表");