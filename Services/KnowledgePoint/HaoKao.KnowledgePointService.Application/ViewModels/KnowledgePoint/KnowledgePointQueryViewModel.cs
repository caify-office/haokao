using HaoKao.KnowledgePointService.Domain.Queries;

namespace HaoKao.KnowledgePointService.Application.ViewModels.KnowledgePoint;

[AutoMapFrom(typeof(KnowledgePointQuery))]
[AutoMapTo(typeof(KnowledgePointQuery))]
public class KnowledgePointQueryViewModel : QueryDtoBase<KnowledgePointQueryListViewModel>
{
    //知识点名称
    public string Name { get; set; }

    public string ChapterNodeId { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.KnowledgePoint))]
[AutoMapTo(typeof(Domain.Entities.KnowledgePoint))]
public class KnowledgePointQueryListViewModel : IDto
{
    /// <summary>
    /// 知识点名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 科目名称
    /// </summary>
    public string SubjectName { get; set; }

    public Guid? Id { get; set; }

    public string Remark { get; set; }

    /// <summary>
    /// 章节Id
    /// </summary>
    public string ChpaterNodeId { get; set; }

    /// <summary>
    /// 章节名称
    /// </summary>
    public string ChpaterNodeName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}