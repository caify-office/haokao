namespace HaoKao.LiveBroadcastService.Domain.Commands.LiveProductPackage;

/// <summary>
/// 删除直播产品包命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteLiveProductPackageCommand(
    Guid Id
) : Command("删除直播产品包");