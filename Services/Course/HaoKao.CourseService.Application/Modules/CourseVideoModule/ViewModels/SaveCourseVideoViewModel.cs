using HaoKao.CourseService.Domain.CourseVideoModule;

namespace HaoKao.CourseService.Application.Modules.CourseVideoModule.ViewModels;

[AutoMapTo(typeof(SaveCourseVideoCommand))]
public record SaveCourseVideoViewModel : IDto
{
    /// <summary>
    /// 关联的课程章节id（阶段学习为课程章节id,智慧辅助学习为课程Id）
    /// </summary>
    [DisplayName("关联的课程章节id（阶段学习为课程章节id,智慧辅助学习为课程Id）")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid CourseChapterId { get; init; }

    /// <summary>
    /// 关联的知识点id(智慧辅助课程专用)
    /// </summary>
    [DisplayName("关联的知识点id(智慧辅助课程专用)")]
    public Guid KnowledgePointId { get; set; }

    /// <summary>
    /// 后缀
    /// </summary>
    [DisplayName("后缀")]
    public string Suffix { get; init; }

    /// <summary>
    /// 时长
    /// </summary>
    [DisplayName("时长")]
    [Required(ErrorMessage = "{0}不能为空")]
    public decimal Duration { get; init; }

    /// <summary>
    /// 是否试听  ture--试听 false --不可试听
    /// </summary>
    [DisplayName("是否试听  ture--试听 false --不可试听")]
    public bool IsTry { get; init; }

    /// <summary>
    /// 视频名称
    /// </summary>
    [DisplayName("视频名称")]
    [Required(ErrorMessage = "{0}不能为空")]

    public string VideoName { get; init; }

    /// <summary>
    /// 视频源名称
    /// </summary>
    [DisplayName("视频源名称")]
    public string SourceName { get; init; }

    /// <summary>
    /// 播放url-冗余
    /// </summary>
    [DisplayName("播放url-冗余")]
    public string VideoUrl { get; init; }

    /// <summary>
    /// 视频id
    /// </summary>
    [DisplayName("视频id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string VideoId { get; init; }

    /// <summary>
    /// 显示名称
    /// </summary>
    [DisplayName("显示名称")]
    public string DisplayName { get; set; }

    /// <summary>
    /// 视频分类id
    /// </summary>
    [DisplayName("视频分类id")]
    public long? CateId { get; set; }

    /// <summary>
    /// 视频分类名称
    /// </summary>
    [DisplayName("视频分类名称")]
    public string CateName { get; set; }

    /// <summary>
    /// 视频标签
    /// </summary>
    [DisplayName("视频标签")]
    public string Tags { get; set; }
}