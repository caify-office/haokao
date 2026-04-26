using HaoKao.LiveBroadcastService.Domain.Enums;

namespace HaoKao.LiveBroadcastService.Domain.Commands.LiveProductPackage;

/// <summary>
/// 创建直播产品包命令
/// </summary>
public record CreateLiveProductPackageCommand(
    List<CreateLiveProductPackageModel> models
) : Command("批量创建直播产品包");

/// <param name="LiveVideoId">所属视频直播Id</param>
/// <param name="ProductPackageId">产品包Id</param>
/// <param name="ProductPackageName">产品包名称</param>
/// <param name="ProductType">产品包类别</param>
/// <param name="Sort">排序</param>
public record CreateLiveProductPackageModel(
    Guid LiveVideoId,
    Guid ProductPackageId,
    string ProductPackageName,
    ProductType ProductType,
    int Sort
);