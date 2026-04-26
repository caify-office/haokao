namespace HaoKao.PaperService.Application.ViewModels;

[AutoMapFrom(typeof(Domain.Entities.Paper))]
public class BrowsePaperStructViewModel : IDto
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
    /// 及格分数
    /// </summary>
    public decimal Score { get; set; }

    /// <summary>
    /// 总分
    /// </summary>
    public decimal TotalScore { get; set; }

    /// <summary>
    /// 试卷结构json
    /// </summary>
    [JsonIgnore]
    public string StructJson { get; set; }

    /// <summary>
    /// 试卷结构
    /// </summary>
    [JsonPropertyName("structJson")]
    public PaperStructViewModel StructJsonViewModel => JsonSerializer.Deserialize<PaperStructViewModel>(StructJson);
}