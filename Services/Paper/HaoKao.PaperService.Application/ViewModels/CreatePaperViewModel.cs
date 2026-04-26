using HaoKao.PaperService.Domain.Commands;
using HaoKao.PaperService.Domain.Enumerations;

namespace HaoKao.PaperService.Application.ViewModels;

[AutoMapTo(typeof(CreatePaperCommand))]
public record CreatePaperViewModel : IDto
{
    /// <summary>
    /// 试卷名称
    /// </summary>
    [DisplayName("试卷名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Name { get; init; }

    /// <summary>
    /// 科目id
    /// </summary>
    [DisplayName("科目id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid SubjectId { get; init; }

    /// <summary>
    /// 科目名称
    /// </summary>
    [DisplayName("科目名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string SubjectName { get; init; }

    /// <summary>
    /// 所属分类
    /// </summary>
    [DisplayName("所属分类")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid CategoryId { get; init; }

    /// <summary>
    /// 分类名称
    /// </summary>
    [DisplayName("分类名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string CategoryName { get; init; }

    /// <summary>
    /// 考试时长
    /// </summary>
    [DisplayName("考试时长")]
    public int Time { get; init; }

    /// <summary>
    /// 及格分数
    /// </summary>
    [DisplayName("及格分数")]
    public decimal Score { get; init; }

    /// <summary>
    /// 是否限免  1--不限免 2--限免
    /// </summary>
    [DisplayName("是否限免  1--不限免 2--限免")]
    public FreeEnum IsFree { get; init; }

    /// <summary>
    /// 发布状态 1-未发布 2-已发布
    /// </summary>
    [DisplayName("发布状态 1-未发布 2-已发布")]
    public StateEnum State { get; init; }

    /// <summary>
    /// 排序
    /// </summary>
    [DisplayName("排序")]
    public int Sort { get; init; }

    /// <summary>
    /// 年份
    /// </summary>
    [DisplayName("年份")]
    public int Year { get; init; }
}