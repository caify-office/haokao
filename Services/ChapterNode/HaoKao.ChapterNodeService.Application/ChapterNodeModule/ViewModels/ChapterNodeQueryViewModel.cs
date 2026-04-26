using HaoKao.ChapterNodeService.Domain.ChapterNodeModule;

namespace HaoKao.ChapterNodeService.Application.ChapterNodeModule.ViewModels;

[AutoMapFrom(typeof(ChapterNodeQuery))]
[AutoMapTo(typeof(ChapterNodeQuery))]
public class ChapterNodeQueryViewModel : QueryDtoBase<ChapterNodeQueryListViewModel>
{
    public Guid? Id { get; init; }

    /// <summary>
    /// 科目Id
    /// </summary>
    public Guid? SubjectId { get; init; }

    /// <summary>
    ///父级Id
    /// </summary>
    public Guid? ParentId { get; init; }
}

[AutoMapFrom(typeof(ChapterNode))]
[AutoMapTo(typeof(ChapterNode))]
public record ChapterNodeQueryListViewModel : IDto
{
    public Guid? Id { get; init; }

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
    public List<Guid> ParentIds { get; init; }

    /// <summary>
    /// 父级Id
    /// </summary>
    public Guid ParentId { get; init; }

    /// <summary>
    /// 父级名称
    /// </summary>
    public string ParentName { get; init; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int Sort { get; init; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; init; }
}