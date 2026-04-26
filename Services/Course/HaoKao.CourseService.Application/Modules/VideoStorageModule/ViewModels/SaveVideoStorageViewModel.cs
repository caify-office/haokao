namespace HaoKao.CourseService.Application.Modules.VideoStorageModule.ViewModels;

public record SaveVideoStorageViewModel : IDto
{
    /// <summary>
    /// 视频存储器ID
    /// </summary>
    [DisplayName("视频存储器ID")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid VideoStorageHandlerId { get; init; }

    /// <summary>
    /// 视频存储器名称
    /// </summary>
    [DisplayName("视频存储器名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string VideoStorageHandlerName { get; init; }

    /// <summary>
    /// 相关的配置参数
    /// </summary>
    [DisplayName("相关的配置参数")]
    [Required(ErrorMessage = "{0}不能为空")]
    public dynamic ConfigParameter { get; init; }
}