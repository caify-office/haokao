namespace HaoKao.KnowledgePointService.Application.ViewModels.KnowledgePoint;

[AutoMapFrom(typeof(Domain.Entities.KnowledgePoint))]
public class KnowledgePointBrowseViewModel : IDto
{
    /// <summary>
    /// 知识点名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 章节名称
    /// </summary>
    public string ChpaterNodeName { get; set; }

    /// <summary>
    /// 章节id
    /// </summary>
    public string ChpaterNodeId { get; set; }

    /// <summary>
    /// 知识点备注
    /// </summary>
    public string Remark { get; set; }
}