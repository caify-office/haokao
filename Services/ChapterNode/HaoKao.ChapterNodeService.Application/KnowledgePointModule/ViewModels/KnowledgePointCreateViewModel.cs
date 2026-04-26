using HaoKao.ChapterNodeService.Domain.KnowledgePointModule;
using HaoKao.Common.Enums;

namespace HaoKao.ChapterNodeService.Application.KnowledgePointModule.ViewModels;

[AutoMapTo(typeof(CreateKnowledgePointCommand))]
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
    public Guid? ChapterNodeId { get; set; }

    /// <summary>
    /// 章节名称
    /// </summary>
    [DisplayName("章节名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string ChapterNodeName { get; set; }

    /// <summary>
    /// 考试频率
    /// </summary>
    [DisplayName("考试频率")]
    [Required(ErrorMessage = "{0}不能为空")]
    public ExamFrequency ExamFrequency { get; set; }

    /// <summary>
    /// 知识点备注
    /// </summary>
    [DisplayName("知识点备注")]
    public string Remark { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    [DisplayName("排序号")]
    public int Sort { get; set; }
}