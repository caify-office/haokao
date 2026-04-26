using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HaoKao.LearnProgressService.Application.ViewModels.LearnProgress;


[AutoMapTo(typeof(Domain.Entities.LearnProgress))]
public class CreateLearnProgressViewModel : IDto
{
    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId { get; set; } = Guid.Empty;
    /// <summary>
    /// 对应的科目Id
    /// </summary>
    public Guid SubjectId { get; set; } = Guid.Empty;
    /// <summary>
    /// 章节id
    /// </summary>
    [DisplayName("章节id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid ChapterId{ get; set; }

    /// <summary>
    /// 课程id
    /// </summary>
    [DisplayName("课程id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid CourseId{ get; set; }

    /// <summary>
    /// 视频id
    /// </summary>
    [DisplayName("视频id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string VideoId{ get; set; }

    /// <summary>
    /// 当前视频标识符,
    /// </summary>
    [DisplayName("当前视频标识符,")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string Identifier{ get; set; }

    /// <summary>
    /// 学习时长(单位:s)
    /// </summary>
    [DisplayName("学习时长(单位:s)")]
    [Required(ErrorMessage = "{0}不能为空")]
    public float Progress { get; set; }

    /// <summary>
    /// 视频总长度(单位:s)--冗余,用于进度百分比计算(百分比计算前端处理)
    /// </summary>
    [DisplayName("视频总长度(单位:s)--冗余,用于进度百分比计算")]
    [Required(ErrorMessage = "{0}不能为空")]
    public float TotalProgress { get; set; }

    /// <summary>
    /// 观看视频最大长度(单位:s)
    /// </summary>
    [DisplayName("观看视频最大长度(单位:s)")]
    [Required(ErrorMessage = "{0}不能为空")]
    public float MaxProgress { get; set; }
}