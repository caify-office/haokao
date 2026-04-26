using Girvs.BusinessBasis.Dto;

namespace HaoKao.Common.RemoteModel;

public class BrowsePaperInfo : BaseBrowsePaperInfo, IDto
{
    /// <summary>
    /// 返回题型列表
    /// </summary>
    public List<dynamic> QuestionTypes { get; set; }
}

public class BaseBrowsePaperInfo
{
    /// <summary>
    /// 试卷id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 试卷名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 科目id
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 科目名称
    /// </summary>
    public string SubjectName { get; set; }

    /// <summary>
    /// 所属分类
    /// </summary>
    public Guid CategoryId { get; set; }

    /// <summary>
    /// 分类名称
    /// </summary>
    public string CategoryName { get; set; }

    /// <summary>
    /// 考试时长
    /// </summary>
    public int Time { get; set; }

    /// <summary>
    /// 及格分数
    /// </summary>
    public decimal Score { get; set; }

    /// <summary>
    /// 是否限免  1--不限免 2--限免
    /// </summary>
    public int IsFree { get; set; }

    /// <summary>
    /// 发布状态 1-未发布 2-已发布
    /// </summary>
    public int State { get; set; }

    /// <summary>
    /// 总题数
    /// </summary>
    public int QuestionCount { get; set; }

    /// <summary>
    /// 总分数
    /// </summary>
    public decimal TotalScore { get; set; }

    /// <summary>
    /// 试卷结构json
    /// </summary>
    public dynamic StructJson { get; set; }
}