using HaoKao.StudyMaterialService.Domain.Entities;
using HaoKao.StudyMaterialService.Domain.Queries;

namespace HaoKao.StudyMaterialService.Application.ViewModels;

[AutoMapFrom(typeof(StudyMaterialQuery))]
[AutoMapTo(typeof(StudyMaterialQuery))]
public class QueryStudyMaterialViewModel : QueryDtoBase<BrowseStudyMaterialViewModel>
{
    /// <summary>
    /// 资料名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 所属科目
    /// </summary>
    public Guid? SubjectId { get; set; }

    /// <summary>
    /// 年份
    /// </summary>
    public string Year { get; set; }
}

[AutoMapFrom(typeof(StudyMaterial))]
[AutoMapTo(typeof(StudyMaterial))]
public record BrowseStudyMaterialViewModel : IDto
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 资料名称
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 年份
    /// </summary>
    public int Year { get; init; }

    /// <summary>
    /// 启用/禁用
    /// </summary>
    public bool Enable { get; init; }

    /// <summary>
    /// 资料内容
    /// </summary>
    public List<Material> Materials { get; init; }

    /// <summary>
    /// 科目
    /// </summary>
    public string Subjects { get; init; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [JsonIgnore]
    public DateTime CreateTime { get; init; }
}