using HaoKao.LiveBroadcastService.Domain.Enums;

namespace HaoKao.LiveBroadcastService.Application.ViewModels.LiveProductPackage;

[AutoMapFrom(typeof(Domain.Entities.LiveProductPackage))]
public class BrowseLiveProductPackageViewModel : IDto
{
    /// <summary>
    /// 所属视频直播
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 所属视频直播Id
    /// </summary>
    public Guid LiveVideoId { get; set; }

    /// <summary>
    /// 产品包Id
    /// </summary>
    public Guid ProductPackageId { get; set; }

    /// <summary>
    /// 产品包名称
    /// </summary>
    public string ProductPackageName { get; set; }

    /// <summary>
    /// 产品包类别
    /// </summary>
    public ProductType ProductType { get; set; }

    /// <summary>
    /// 是否上架
    /// </summary>
    public bool IsShelves { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}