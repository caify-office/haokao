using System.Linq;

namespace HaoKao.LearnProgressService.Application.ViewModels.LearnProgress;


public class CourseLearningProgressStatisticsViewModel
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
    /// 课程数
    /// </summary>
    public int CouseCount { get; set; }

    /// <summary>
    /// 已学完课程数
    /// </summary>
    public int CouseIsEndCount { get; set; }

    public List<CouseCategoryStaticsViewModel> Result { get; set; }

    /// <summary>
    /// 总完成度
    /// </summary>
    public float TotalCompletionDegree
    {
        get
        {
            if (!Result.Any()) return 0;
            return (float)Result.Sum(x=>x.MaxProgress) / (float)Result.Sum(x => x.Duration);
        }
    }
}
public class CouseCategoryStaticsViewModel
{
    /// <summary>
    /// 阶段id
    /// </summary>
    public Guid Category { get; set; }
    /// <summary>
    /// 阶段总课程时长
    /// </summary>
    public float Duration { get; set; }
    /// <summary>
    /// 阶段已学时长
    /// </summary>
    public float MaxProgress { get; set; }
    /// <summary>
    /// 完成度
    /// </summary>
    public float CompletionDegree {
        get
        { 
            if(Duration==0f) return 0f;
          return (float)MaxProgress / (float)Duration;
        } 
    }

}


