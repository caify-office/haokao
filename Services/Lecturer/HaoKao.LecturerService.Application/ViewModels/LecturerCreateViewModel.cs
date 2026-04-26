using HaoKao.LecturerService.Domain.Commands;
using System.Collections.Generic;

namespace HaoKao.LecturerService.Application.ViewModels;

[AutoMapTo(typeof(CreateLecturerCommand))]
public record  CreateLecturerViewModel : IDto
{
    /// <summary>
    /// 教师姓名
    /// </summary>
    [DisplayName("教师姓名")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Name { get; init; }

    /// <summary>
    /// 科目Id数组
    /// </summary>
    [DisplayName("科目Id数组")]
    [Required(ErrorMessage = "{0}不能为空")]
    public List<Guid> SubjectIds { get; init; }

    /// <summary>
    /// 科目名称数组
    /// </summary>
    [DisplayName("科目名称数组")]
    [Required(ErrorMessage = "{0}不能为空")]
    public List<string> SubjectNames { get; init; }

    /// <summary>
    /// 简介
    /// </summary>
    [DisplayName("简介")]
    public string Desc { get; init; }

    /// <summary>
    /// 课程介绍
    /// </summary>
    [DisplayName("课程介绍")]
    public string CourseIntroduction { get; init; }

    /// <summary>
    /// 头像
    /// </summary>
    [DisplayName("头像")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(500, ErrorMessage = "{0}长度不能大于{1}")]
    public string HeadPortraitUrl { get; init; }

    /// <summary>
    /// 形象照
    /// </summary>
    [DisplayName("形象照")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(500, ErrorMessage = "{0}长度不能大于{1}")]
    public string PhotoUrl { get; init; }

    /// <summary>
    /// 排序
    /// </summary>
    [DisplayName("排序")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int Sort { get; init; }

    /// <summary>
    /// 关联的产品包id
    /// </summary>
    [DisplayName("关联的产品包id")]
    public List<Guid> ProductPackageIds { get; init; }
}