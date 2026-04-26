using HaoKao.ChapterNodeService.Domain.ChapterNodeModule;
using HaoKao.ChapterNodeService.Domain.KnowledgePointModule;

namespace HaoKao.ChapterNodeService.Application.ChapterNodeModule.ViewModels;

[AutoMapFrom(typeof(ChapterNode))]
public record BrowseChapterNodeViewModel : IDto
{
    public Guid Id { get; set; }
    /// <summary>
    /// 科目Id
    /// </summary>
    public Guid SubjectId { get; init; }

    /// <summary>
    /// 编码
    /// </summary>
    public string Code { get; init; }

    /// <summary>
    /// 章节名称
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 父级Id集合
    /// </summary>
    public IReadOnlyList<Guid> ParentIds { get; init; }

    /// <summary>
    /// 父级Id
    /// </summary>
    public Guid ParentId { get; init; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int Sort { get; init; }

    /// <summary>
    /// 子章节
    /// </summary>
    public List<ChapterNode> Children { get; set; }
    /// <summary>
    /// 章节下知识点树
    /// </summary>
    public List<KnowledgePoint> knowledgePoints { get; set; }
}