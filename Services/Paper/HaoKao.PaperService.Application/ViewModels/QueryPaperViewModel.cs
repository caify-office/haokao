using HaoKao.PaperService.Domain.Enumerations;
using HaoKao.PaperService.Domain.Queries;

namespace HaoKao.PaperService.Application.ViewModels;

[AutoMapFrom(typeof(PaperQuery))]
[AutoMapTo(typeof(PaperQuery))]
public class PaperQueryViewModel : QueryDtoBase<PaperQueryListViewModel>
{
    /// <summary>
    /// 试卷名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 科目id
    /// </summary>
    public Guid? SubjectId { get; set; }

    /// <summary>
    /// 类别id
    /// </summary>
    public Guid? CategoryId { get; set; }

    /// <summary>
    /// 发布状态 1--未发布 2--已发布
    /// </summary>
    public StateEnum State { get; set; }

    /// <summary>
    /// 是否限免 1-不限免 2-限免
    /// </summary>
    public FreeEnum IsFree { get; set; }

    /// <summary>
    /// 年份
    /// </summary>
    public int? Year { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.Paper))]
[AutoMapTo(typeof(Domain.Entities.Paper))]
public class PaperQueryListViewModel : IDto
{
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
    public FreeEnum IsFree { get; set; }

    /// <summary>
    /// 发布状态 1-未发布 2-已发布
    /// </summary>
    public StateEnum State { get; set; }

    /// <summary>
    /// 模板json--存储模板--模板明细json
    /// </summary>
    public string StructJson { get; set; }

    /// <summary>
    /// 题目总数
    /// </summary>
    public int QuestionCount { get; set; }

    /// <summary>
    /// 总分
    /// </summary>
    public decimal TotalScore { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 年份
    /// </summary>
    public int Year { get; set; }
}