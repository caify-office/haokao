namespace HaoKao.LiveBroadcastService.Domain.Commands.LiveProductPackage;
/// <summary>
/// 修改产品上下架
/// </summary>
/// <param name="Ids"></param>
/// <param name="IsShelves">产品上下架</param>
public record SetLiveProductPackageShelvesCommand(ICollection<Guid> Ids, bool IsShelves) : Command("修改产品上下架");