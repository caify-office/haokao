using HaoKao.StudyMaterialService.Domain.Commands;
using HaoKao.StudyMaterialService.Domain.Entities;

namespace HaoKao.StudyMaterialService.Application.ViewModels;

[AutoMapTo(typeof(CreateStudyMaterialCommand))]
public record CreateStudyMaterialViewModel : IDto
{
    /// <summary>
    /// 资料名称
    /// </summary>
    [DisplayName("资料名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Name { get; init; }

    /// <summary>
    /// 年份
    /// </summary>
    [DisplayName("年份")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int Year { get; init; }

    /// <summary>
    /// 启用/禁用
    /// </summary>
    [DisplayName("启用/禁用")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool Enable { get; init; }

    /// <summary>
    /// 资料内容
    /// </summary>
    [DisplayName("资料内容")]
    [Required(ErrorMessage = "{0}不能为空")]
    public List<Material> Materials { get; init; }

    /// <summary>
    /// 科目
    /// </summary>
    [DisplayName("科目")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string Subjects { get; init; }
}

[AutoMapTo(typeof(UpdateStudyMaterialCommand))]
public record UpdateStudyMaterialViewModel : CreateStudyMaterialViewModel
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [DisplayName("主键Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid Id { get; set; }
}