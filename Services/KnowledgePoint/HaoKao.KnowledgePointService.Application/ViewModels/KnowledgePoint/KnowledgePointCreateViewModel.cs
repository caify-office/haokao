using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HaoKao.KnowledgePointService.Application.ViewModels.KnowledgePoint;

[AutoMapTo(typeof(Domain.Entities.KnowledgePoint))]
public class KnowledgePointCreateViewModel : IDto
{
    /// <summary>
    /// 知识点名称
    /// </summary>
    [DisplayName("知识点名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Name { get; set; }

    /// <summary>
    /// 科目名称
    /// </summary>
    [DisplayName("科目名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string SubjectName { get; set; }

    /// <summary>
    /// 章节Id
    /// </summary>
    [DisplayName("章节Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid? ChpaterNodeId { get; set; }

    /// <summary>
    /// 章节名称
    /// </summary>
    [DisplayName("章节名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string ChpaterNodeName { get; set; }

    /// <summary>
    /// 知识点备注
    /// </summary>
    [DisplayName("知识点备注")]
    public string Remark { get; set; }
}