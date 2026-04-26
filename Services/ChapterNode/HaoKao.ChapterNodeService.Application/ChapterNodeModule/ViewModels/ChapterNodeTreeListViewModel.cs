using HaoKao.ChapterNodeService.Domain.ChapterNodeModule;

namespace HaoKao.ChapterNodeService.Application.ChapterNodeModule.ViewModels;

/// <summary>
/// 章节树返回
/// </summary>
[AutoMapFrom(typeof(ChapterNode))]
public class ChapterNodeTreeListViewModel : IDto
{
    /// <summary>
    /// 主键
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 章节名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 父级Id集合
    /// </summary>
    public List<Guid> ParentIds { get; set; }

    /// <summary>
    /// 父级Id
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int Sort { get; set; }

    //private List<ChapterNodeTreeListViewModel> chidren;
    ///// <summary>
    ///// 所属子节点
    ///// </summary>
    //public List<ChapterNodeTreeListViewModel> Children
    //{
    //    get
    //    {
    //        return chidren;
    //    }

    //    set
    //    {
    //        if (value == null || value.Count == 0)
    //        {
    //            chidren = null;
    //        }
    //        else
    //        {
    //            chidren = value;
    //        }
    //    }
    //}
}