namespace HaoKao.LearnProgressService.Application.ViewModels.LearnProgress;


[AutoMapFrom(typeof(Domain.Entities.LearnProgress))]
public class BrowseLearnProgressViewModel : IDto
{
    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId { get; set; }
    /// <summary>
    /// 对应的科目Id
    /// </summary>
    public Guid SubjectId { get; set; }
    /// <summary>
    /// 章节id
    /// </summary>
    public Guid ChapterId{ get; set; }

    /// <summary>
    /// 课程id
    /// </summary>
    public Guid CourseId{ get; set; }

    /// <summary>
    /// 视频id
    /// </summary>
    public Guid VideoId{ get; set; }

    /// <summary>
    /// 当前视频标识符,
    /// </summary>
    public string Identifier{ get; set; }

    /// <summary>
    /// 学习时长(单位:s)
    /// </summary>
    public float Progress { get; set; }

    /// <summary>
    /// 视频总长度(单位:s)--冗余,用于进度百分比计算
    /// </summary>
    public float TotalProgress { get; set; }

    /// <summary>
    /// 观看视频最大长度(单位:s)
    /// </summary>
    public float MaxProgress { get; set; }
}
