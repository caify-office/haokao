using HaoKao.StudentService.Domain.Commands;
using HaoKao.StudentService.Domain.Entities;
using HaoKao.StudentService.Domain.Queries;

namespace HaoKao.StudentService.Application.ViewModels;

[AutoMapFrom(typeof(StudentParameterConfigQuery))]
[AutoMapTo(typeof(StudentParameterConfigQuery))]
public class QueryStudentParameterConfigViewModel : QueryDtoBase<BrowseStudentParameterConfigViewModel>
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid? UserId { get; init; }

    /// <summary>
    /// 设置值字段名称
    /// </summary>
    public string PropertyName { get; init; }

    /// <summary>
    /// 设置值类型
    /// </summary>
    public string PropertyType { get; init; }

    /// <summary>
    /// 设置值
    /// </summary>
    public string PropertyValue { get; init; }
}

public record StudentParameterConfigQueryEffectiveNextDayViewModel
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid? UserId { get; init; }

    /// <summary>
    /// 设置值字段名称
    /// </summary>
    public string PropertyName { get; init; }

    /// <summary>
    /// 设置值类型
    /// </summary>
    public string PropertyType { get; init; }
}

[AutoMapTo(typeof(StudentParameterConfig))]
[AutoMapFrom(typeof(StudentParameterConfig))]
public record BrowseStudentParameterConfigViewModel : IDto
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName { get; init; }

    /// <summary>
    /// 设置值字段名称
    /// </summary>
    public string PropertyName { get; init; }

    /// <summary>
    /// 设置值类型
    /// </summary>
    public string PropertyType { get; init; }

    /// <summary>
    /// 设置值
    /// </summary>
    public string PropertyValue { get; init; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Desc { get; init; }
}

[AutoMapTo(typeof(SaveStudentParameterConfigCommand))]
public record CreateStudentParameterConfigViewModel : IDto
{
    /// <summary>
    /// 用户Id
    /// </summary>
    [DisplayName("用户Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid UserId { get; init; }

    /// <summary>
    /// 昵称
    /// </summary>
    [DisplayName("昵称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string NickName { get; init; }

    /// <summary>
    /// 设置值字段名称
    /// </summary>
    [DisplayName("设置值字段名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string PropertyName { get; init; }

    /// <summary>
    /// 设置值类型
    /// </summary>
    [DisplayName("设置值类型")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(500, ErrorMessage = "{0}长度不能大于{1}")]
    public string PropertyType { get; init; }

    /// <summary>
    /// 设置值
    /// </summary>
    [DisplayName("设置值")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(500, ErrorMessage = "{0}长度不能大于{1}")]
    public string PropertyValue { get; init; }

    /// <summary>
    /// 描述
    /// </summary>
    [DisplayName("描述")]
    [MaxLength(1000, ErrorMessage = "{0}长度不能大于{1}")]
    public string Desc { get; init; }
}

[AutoMapTo(typeof(UpdateStudentParameterConfigCommand))]
public record UpdateStudentParameterConfigViewModel : CreateStudentParameterConfigViewModel
{
    /// <summary>
    /// Id
    /// </summary>
    [DisplayName("Id")]
    public Guid Id { get; init; }
}