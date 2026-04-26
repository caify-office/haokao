using HaoKao.ChapterNodeService.Domain.KnowledgePointModule;
using HaoKao.Common.Enums;

namespace HaoKao.ChapterNodeService.Application.KnowledgePointModule.ViewModels;

[AutoMapFrom(typeof(KnowledgePointQuery))]
[AutoMapTo(typeof(KnowledgePointQuery))]
public class KnowledgePointQueryViewModel : QueryDtoBase<KnowledgePointQueryListViewModel>
{
    //知识点名称
    public string Name { get; set; }

    public Guid? ChapterNodeId { get; set; }

    public Guid? SubjectId { get; set; }
}

[AutoMapFrom(typeof(KnowledgePoint))]
[AutoMapTo(typeof(KnowledgePoint))]
public class KnowledgePointQueryListViewModel : IDto
{
    public Guid Id { get; set; }
    /// <summary>
    /// 知识点名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 科目名称
    /// </summary>
    public string SubjectName { get; set; }

    public string Remark { get; set; }

    /// <summary>
    /// 章节Id
    /// </summary>
    public string ChapterNodeId { get; set; }

    /// <summary>
    /// 章节名称
    /// </summary>
    public string ChapterNodeName { get; set; }


    /// <summary>
    /// 考试频率
    /// </summary>
    public ExamFrequency ExamFrequency { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    [DisplayName("排序号")]
    public int Sort { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}