using HaoKao.CourseService.Domain.VideoStorageModule;

namespace HaoKao.CourseService.Application.Modules.VideoStorageModule.ViewModels;

[AutoMapFrom(typeof(VideoStorage))]
public record BrowseVideoStorageViewModel : IDto
{
    /// <summary>
    /// 视频存储器ID
    /// </summary>
    public Guid VideoStorageHandlerId { get; init; }

    /// <summary>
    /// 视频存储器名称
    /// </summary>
    public string VideoStorageHandlerName { get; init; }

    /// <summary>
    /// 相关的配置参数
    /// </summary>
    public dynamic ConfigParameter { get; set; }
}