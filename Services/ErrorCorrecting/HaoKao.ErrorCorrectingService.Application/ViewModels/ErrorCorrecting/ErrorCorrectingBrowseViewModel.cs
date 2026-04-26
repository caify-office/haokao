using HaoKao.ErrorCorrectingService.Domain.Entities;

namespace HaoKao.ErrorCorrectingService.Application.ViewModels.ErrorCorrecting;

[AutoMapFrom(typeof(Domain.Entities.ErrorCorrecting))]
public class BrowseErrorCorrectingViewModel : IDto
{
    /// <summary>
    /// 问题id
    /// </summary>
    public Guid QuestionId { get; set; }

    /// <summary>
    /// 用户id
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 问题类型
    /// </summary>
    public string QuestionTypes { get; set; }

    public Guid SubjectId { get; set; } //科目ID

    public string SubjectName { get; set; } //科目名称

    public Guid QuestionTypeId { get; set; } //题型类别id

    public string QuestionTypeName { get; set; } //题型类别名称

    public string QuestionText { get; set; } //题干

    public string NickName { get; set; } //昵称

    public string Phone { get; set; } //手机号码

    public Guid CategoryId { get; set; } //分类id

    public string CategoryName { get; set; } //分类名称

    public StatusEnum Status { get; set; } //分类名称
}