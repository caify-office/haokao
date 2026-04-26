namespace HaoKao.CourseService.Application.Modules.VideoStorageModule.ViewModels;

/// <summary>
/// 视频存储处理器模型
/// </summary>
public record VideoStorageHandlerViewModel
{
    /// <summary>
    /// 存储处理Id
    /// </summary>
    public Guid StorageHandlerId { get; init; }

    /// <summary>
    /// 存储处理名称
    /// </summary>
    public string StorageHandlerName { get; init; }

    /// <summary>
    /// 存储处理配置
    /// </summary>
    public dynamic StorageConfig { get; init; }
}

public record VideoModel
{
    /// <summary>
    /// 请求ID。
    /// </summary>
    public string RequestId { get; init; }

    /// <summary>
    /// 视频播放凭证
    /// </summary>
    public string PlayAuth { get; init; }

    /// <summary>
    /// 视频标题
    /// </summary>
    public string Title { get; init; }

    /// <summary>
    /// 视频ID
    /// </summary>
    public string VideoId { get; init; }

    /// <summary>
    /// 视频封面
    /// </summary>
    public string CoverURL { get; init; }

    /// <summary>
    /// 视频时长
    /// </summary>
    public float? Duration { get; init; }

    /// <summary>
    /// 视频状态
    /// Uploading	上传中	视频的初始状态，表示正在上传。
    /// UploadFail	上传失败	由于是断点续传，无法确定上传是否失败，故暂不会出现此值。
    /// UploadSucc	上传完成	-
    /// Transcoding	转码中	-
    /// TranscodeFail	转码失败	转码失败，一般是原片有问题，可在事件通知的 转码完成消息 得到ErrorMessage失败信息，或提交工单联系我们。
    /// Checking	审核中	在 视频点播控制台 > 全局设置 > 审核设置 开启了 先审后发，转码成功后视频状态会变成审核中，此时视频只能在控制台播放。
    /// Blocked	屏蔽	在审核时屏蔽视频。
    /// Normal	正常	视频可正常播放。
    /// </summary>
    public string Status { get; init; }
}

public record VideoInfoModel
{
    /// <summary>
    /// 视频标题
    /// </summary>
    public string Title { get; init; }

    /// <summary>
    /// 视频分类id
    /// </summary>
    public long? CateId { get; init; }

    /// <summary>
    /// 视频分类名称
    /// </summary>
    public string CateName { get; init; }

    /// <summary>
    /// 视频标签
    /// </summary>
    public string Tags { get; init; }

    /// <summary>
    /// 视频ID
    /// </summary>
    public string VideoId { get; init; }
}

public record VideoPlayInfo
{
    public long? Size { get; init; }

    public string Duration { get; init; }

    public string Definition { get; init; }

    public string PlayURL { get; init; }

    public long? Height { get; init; }

    public long? Width { get; init; }
}