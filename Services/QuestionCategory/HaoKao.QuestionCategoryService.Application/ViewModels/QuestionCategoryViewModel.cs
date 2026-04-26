using HaoKao.QuestionCategoryService.Domain.Commands;
using HaoKao.QuestionCategoryService.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HaoKao.QuestionCategoryService.Domain.Queries;
using HaoKao.QuestionCategoryService.Domain.Entities;

namespace HaoKao.QuestionCategoryService.Application.ViewModels;

[AutoMapFrom(typeof(QuestionCategoryQuery))]
[AutoMapTo(typeof(QuestionCategoryQuery))]
public class QuestionCategoryQueryViewModel : QueryDtoBase<BrowseQuestionCategoryViewModel>
{
    /// <summary>
    /// 题库类别名称
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 适应场景
    /// </summary>
    public AdaptPlace? AdaptPlace { get; init; }
}


[AutoMapFrom(typeof(QuestionCategory))]
public record BrowseQuestionCategoryViewModel : IDto
{
    /// <summary>
    /// 主键
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 类名名称
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 类别代码
    /// </summary>
    public string Code { get; init; }

    /// <summary>
    /// 适应场景
    /// </summary>
    public AdaptPlace AdaptPlace { get; init; }

    /// <summary>
    /// 显示条件
    /// </summary>
    public DisplayConditionEnum DisplayCondition { get; init; }

    /// <summary>
    /// 产品包Id(购买跳转对象)
    /// </summary>
    public Guid ProductPackageId { get; init; }

    /// <summary>
    /// 产品包类型
    /// </summary>
    public ProductPackageType? ProductPackageType { get; init; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; init; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; init; }
}


[AutoMapTo(typeof(CreateQuestionCategoryCommand))]
public record CreateQuestionCategoryViewModel : IDto
{
    /// <summary>
    /// 类名名称
    /// </summary>
    [DisplayName("类名名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Name { get; init; }

    /// <summary>
    /// 类别代码
    /// </summary>
    [DisplayName("类别代码")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Code { get; init; }

    /// <summary>
    /// 适应场景
    /// </summary>
    [DisplayName("适应场景")]
    [Required(ErrorMessage = "{0}不能为空")]
    public AdaptPlace AdaptPlace { get; init; }

    /// <summary>
    /// 是否显示
    /// </summary>
    [DisplayName("是否显示")]
    [Required(ErrorMessage = "{0}不能为空")]
    public DisplayConditionEnum DisplayCondition { get; init; }

    /// <summary>
    /// 产品包Id(购买跳转对象)
    /// </summary>
    [DisplayName("产品包Id(购买跳转对象)")]
    public Guid ProductPackageId { get; init; }

    /// <summary>
    /// 产品包类型
    /// </summary>
    [DisplayName("产品包类型")]
    public ProductPackageType? ProductPackageType { get; init; }
}


[AutoMapTo(typeof(UpdateQuestionCategoryCommand))]
public record UpdateQuestionCategoryViewModel : CreateQuestionCategoryViewModel
{
    [DisplayName("Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid Id { get; init; }
}
