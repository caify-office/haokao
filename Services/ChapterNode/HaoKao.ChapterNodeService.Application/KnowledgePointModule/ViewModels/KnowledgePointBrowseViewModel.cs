using HaoKao.ChapterNodeService.Domain.KnowledgePointModule;
using HaoKao.Common.Enums;

namespace HaoKao.ChapterNodeService.Application.KnowledgePointModule.ViewModels;


[AutoMapFrom(typeof(KnowledgePoint))]
public class KnowledgePointBrowseViewModel : IDto
{
    /// <summary>
    /// 知识点名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 章节名称
    /// </summary>
    public string ChapterNodeName { get; set; }

    /// <summary>
    /// 章节id
    /// </summary>
    public string ChapterNodeId { get; set; }

    /// <summary>
    /// 考试频率
    /// </summary>
    public ExamFrequency ExamFrequency { get; set; }

    /// <summary>
    /// 知识点备注
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int Sort { get; set; }
}