using HaoKao.LecturerService.Domain.Queries;

namespace HaoKao.LecturerService.Application.ViewModels;

[AutoMapFrom(typeof(LecturerQuery))]
[AutoMapTo(typeof(LecturerQuery))]
public class LecturerQueryViewModel : QueryDtoBase<BrowseLecturerViewModel>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 所属科目
    /// </summary>
    public Guid? SubjectId { get; set; }

    /// <summary>
    /// 简介
    /// </summary>
    public string Desc { get; set; }

    /// <summary>
    /// 课程介绍
    /// </summary>
    public string CourseIntroduction { get; set; }
}