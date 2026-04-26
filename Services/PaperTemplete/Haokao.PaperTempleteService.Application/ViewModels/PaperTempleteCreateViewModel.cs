using HaoKao.PaperTempleteService.Domain.Entities;

namespace HaoKao.PaperTempleteService.Application.ViewModels;

[AutoMapTo(typeof(PaperTemplete))]
public record CreatePaperTempleteViewModel : IDto
{
    /// <summary>
    /// 试卷模板Id
    /// </summary>
    [DisplayName("试卷模板Id")]
    public Guid? Id { get; init; }

    /// <summary>
    /// 试卷模板名称
    /// </summary>
    [DisplayName("试卷模板名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string TempleteName { get; init; }

    /// <summary>
    /// 备注
    /// </summary>
    [DisplayName("备注")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(100, ErrorMessage = "{0}长度不能大于{1}")]
    public string Remark { get; init; }

    /// <summary>
    /// 模板适用科目
    /// </summary>
    [DisplayName("模板适用科目")]
    public string SuitableSubjects { get; init; }
}