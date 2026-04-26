using HaoKao.CourseFeatureService.Domain;

namespace HaoKao.CourseFeatureService.Application.ViewModels;

[AutoMapFrom(typeof(CourseFeatureQuery))]
[AutoMapTo(typeof(CourseFeatureQuery))]
public class QueryCourseFeatureViewModel : QueryDtoBase<BrowseCourseFeatureViewModel>
{
    /// <summary>
    /// 服务名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool? Enable { get; set; }
}

[AutoMapFrom(typeof(CourseFeature))]
public record BrowseCourseFeatureViewModel : IDto
{
    /// <summary>
    /// 主键
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 服务名称
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 服务内容
    /// </summary>
    public string Content { get; init; }

    /// <summary>
    /// 图标地址
    /// </summary>
    public string IconUrl { get; init; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; init; }

    /// <summary>
    /// 启用/禁用
    /// </summary>
    public bool Enable { get; init; }
}

[AutoMapTo(typeof(CreateCourseFeatureCommand))]
public record CreateCourseFeatureViewModel : IDto
{
    /// <summary>
    /// 服务名称
    /// </summary>
    [DisplayName("服务名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Name { get; init; }

    /// <summary>
    /// 服务内容
    /// </summary>
    [DisplayName("服务内容")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(500, ErrorMessage = "{0}长度不能大于{1}")]
    public string Content { get; init; }

    /// <summary>
    /// 图标地址
    /// </summary>
    [DisplayName("图标地址")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(500, ErrorMessage = "{0}长度不能大于{1}")]
    public string IconUrl { get; init; }

    /// <summary>
    /// 启用/禁用
    /// </summary>
    [DisplayName("启用/禁用")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool Enable { get; init; }
}


[AutoMapTo(typeof(UpdateCourseFeatureCommand))]
public record UpdateCourseFeatureViewModel : CreateCourseFeatureViewModel 
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [DisplayName("主键Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid Id { get; init; }
}

[AutoMapTo(typeof(EnableCourseFeatureCommand))]
public class EnableCourseFeatureViewModel : IDto
{
    /// <summary>
    /// Ids
    /// </summary>
    [DisplayName("Ids")]
    [Required(ErrorMessage = "{0}不能为空")]
    public IReadOnlyList<Guid> Ids { get; set; }

    /// <summary>
    /// 启用/禁用
    /// </summary>
    [DisplayName("启用/禁用")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool Enable { get; set; }
}

[AutoMapFrom(typeof(CourseFeature))]
public class BrowseCourseFeatureWebViewModel : IDto
{
    /// <summary>
    /// 服务名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 服务内容
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 图标地址
    /// </summary>
    public string IconUrl { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [JsonIgnore]
    public DateTime CreateTime { get; set; }
}
