namespace HaoKao.CourseService.Application.Storages.AliyunStorage.Models;

public enum MediaType
{
    video,
    audio,
    image,
    attached,
}

/// <summary>
/// 查询模型
/// </summary>
public class SearchModel
{
    /// <summary>
    /// 查询类型
    /// </summary>
    public string SearchType { get; set; } = "video";

    /// <summary>
    /// 查询字段
    /// </summary>
    public List<string> Fields { get; set; } = ["Title", "CoverURL", "Duration",];

    /// <summary>
    /// 查询条件
    /// </summary>
    public List<string> Match { get; set; }

    /// <summary>
    /// 查询结果排序
    /// </summary>
    public string SortBy { get; set; } = "CreationTime:Desc";

    /// <summary>
    /// 查询当前页
    /// </summary>
    public int PageNo { get; set; } = 1;

    /// <summary>
    /// 查询当前页大小
    /// </summary>
    public int PageSize { get; set; } = 20;
}

public class MediaList
{
    public MediaType MediaType { get; set; }

    public DateTime CreationTime { get; set; }
}