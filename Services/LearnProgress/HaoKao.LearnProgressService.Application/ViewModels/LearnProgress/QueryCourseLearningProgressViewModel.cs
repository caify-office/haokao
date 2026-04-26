namespace HaoKao.LearnProgressService.Application.ViewModels.LearnProgress;


public class QueryCourseLearningProgressViewModel
{
    /// <summary>
    /// 学员id
    /// </summary>
    public Guid StudentId { get; set; }

    /// <summary>
    /// 产品id
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// 科目id
    /// </summary>
    public Guid SubjectId { get; set; }
    /// <summary>
    /// 页码
    /// </summary>
    public int PageIndex { get; set; }
    /// <summary>
    /// 页容积
    /// </summary>
    public int PageSize { get; set; }
    /// <summary>
    /// 总行数
    /// </summary>
    public int RecordCount { get; set; }

    public List<QueryCourseLearningProgressListViewModel> Result { get; set; }
}
public class QueryCourseLearningProgressListViewModel()
{
    public string PermissionName { get; set; }
    /// <summary>
    /// 章节id
    /// </summary>
    public Guid CourseChapterId { get; set; }
    /// <summary>
    /// 章节名称
    /// </summary>
    public string CourseChapterName { get; set; }

    /// <summary>
    /// 总进度
    /// </summary>
    public float Duration { get; set; }
   
    /// <summary>
    /// 最大学习进度
    /// </summary>

    public float MaxProgress { get; set; }
    /// <summary>
    /// 进度
    /// </summary>
    public float Progress
    {
        get
        {
            if (Duration == 0f) return 0f;
            return (float)MaxProgress / Duration;
        }
    }
    /// <summary>
    /// 最后一次听课时间
    /// </summary>
    public DateTime CreateTime { get; set; }

}

