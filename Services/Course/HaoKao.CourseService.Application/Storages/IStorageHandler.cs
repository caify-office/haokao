using HaoKao.CourseService.Application.Modules.VideoStorageModule.ViewModels;
using Newtonsoft.Json;

namespace HaoKao.CourseService.Application.Storages;

public interface IStorageHandler
{
    /// <summary>
    /// 存储器Id
    /// </summary>
    Guid HandlerId { get; }

    /// <summary>
    /// 存储器名称
    /// </summary>
    string HandlerName { get; }

    /// <summary>
    /// 设置配置
    /// </summary>
    /// <param name="jsonConfig"></param>
    void SetConfig(string jsonConfig);

    /// <summary>
    /// 搜索内容
    /// </summary>
    /// <param name="request"></param>
    Task<dynamic> SearchStorage(HttpRequest request);

    /// <summary>
    /// 根据videoId读取播放密钥
    /// </summary>
    /// <param name="videoId"></param>
    /// <returns></returns>
    Task<VideoModel> GetVideoAuth(string videoId);

    /// <summary>
    /// 获取视频信息
    /// </summary>
    /// <param name="videoId"></param>
    /// <returns></returns>
    Task<VideoInfoModel> GetVideoInfo(string videoId);

    /// <summary>
    /// 读取视频的url
    /// </summary>
    /// <param name="videoId"></param>
    /// <returns></returns>
    Task<VideoPlayInfo> GetVideoPlayInfo(string videoId);

    /// <summary>
    /// 获取视频分类
    /// </summary>
    /// <returns></returns>
    Task<dynamic> GetCategories();

    /// <summary>
    /// 获取STS凭证
    /// </summary>
    /// <returns></returns>
    dynamic AssumeRole();
}

/// <summary>
/// 存储器处理基类
/// </summary>
/// <typeparam name="TConfig"></typeparam>
/// <typeparam name="TRequestModel"></typeparam>
public abstract class StorageHandler<TConfig, TRequestModel> : IStorageHandler where TConfig : IStorageConfig
{
    public TConfig Config { get; set; }

    public virtual void SetConfig(string jsonConfig)
    {
        Config = JsonConvert.DeserializeObject<TConfig>(jsonConfig);
    }

    public abstract Task DynamicBindingModel(TRequestModel model);

    public abstract Guid HandlerId { get; }

    public abstract string HandlerName { get; }

    public abstract Task<dynamic> SearchStorage(HttpRequest request);

    public abstract Task<VideoModel> GetVideoAuth(string videoId);

    public abstract Task<VideoInfoModel> GetVideoInfo(string videoId);

    public abstract Task<VideoPlayInfo> GetVideoPlayInfo(string videoId);

    public abstract Task<dynamic> GetCategories();

    public abstract dynamic AssumeRole();
}