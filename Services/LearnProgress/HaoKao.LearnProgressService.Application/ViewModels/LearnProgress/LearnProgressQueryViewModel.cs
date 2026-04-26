using HaoKao.LearnProgressService.Domain.Queries.EntityQuery;

namespace HaoKao.LearnProgressService.Application.ViewModels.LearnProgress;

[AutoMapFrom(typeof(LearnProgressQuery))]
[AutoMapTo(typeof(LearnProgressQuery))]
public class LearnProgressQueryViewModel : QueryDtoBase<LearnProgressQueryListViewModel>
{
    /// <summary>
    /// 用户
    /// </summary>
    public Guid? CreatorId { get; set; }

    /// <summary>
    /// 需要查询的视频集合
    /// </summary>
    public string VideoIds { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.LearnProgress))]
[AutoMapTo(typeof(Domain.Entities.LearnProgress))]
public class LearnProgressQueryListViewModel : IDto
{
    public Guid Id { get; set; }

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
    public Guid ChapterId { get; set; }

    /// <summary>
    /// 课程id
    /// </summary>
    public Guid CourseId { get; set; }

    /// <summary>
    /// 视频id
    /// </summary>
    public string VideoId { get; set; }

    /// <summary>
    /// 当前视频标识符,
    /// </summary>
    public string Identifier { get; set; }

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

    /// <summary>
    /// true-已完成  false--待完成
    /// </summary>

    public bool IsEnd { get; set; }

    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 用来分组的排序时间-年月日
    /// </summary>
    public string SortDate { get; set; }

    /// <summary>
    /// 播放的时间--时分
    /// </summary>
    public string PlayDate { get; set; }
}