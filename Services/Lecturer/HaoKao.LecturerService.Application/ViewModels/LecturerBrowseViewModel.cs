using HaoKao.LecturerService.Domain.Entities;
using System.Collections.Generic;

namespace HaoKao.LecturerService.Application.ViewModels;

[AutoMapTo(typeof(Lecturer))]
[AutoMapFrom(typeof(Lecturer))]
public record BrowseLecturerViewModel : IDto
{
    public Guid Id { get; init; }

    /// <summary>
    /// 教师姓名
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 科目Id数组
    /// </summary>
    public IReadOnlyList<Guid> SubjectIds { get; init; }

    /// <summary>
    /// 科目名称数组
    /// </summary>
    public IReadOnlyList<string> SubjectNames { get; init; }

    /// <summary>
    /// 简介
    /// </summary>
    public string Desc { get; init; }

    /// <summary>
    /// 课程介绍
    /// </summary>
    public string CourseIntroduction { get; init; }

    /// <summary>
    /// 头像
    /// </summary>
    public string HeadPortraitUrl { get; init; }

    /// <summary>
    /// 形象照
    /// </summary>
    public string PhotoUrl { get; init; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; init; }

    /// <summary>
    /// 关联的产品包id
    /// </summary>
    public IReadOnlyList<Guid> ProductPackageIds { get; init; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; init; }
}