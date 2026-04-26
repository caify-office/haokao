using HaoKao.LiveBroadcastService.Domain.Entities;
using System.Data;

namespace HaoKao.LiveBroadcastService.Domain.Repositories;

public interface ILiveReservationRepository : IRepository<LiveReservation>
{
    Task<Dictionary<Guid, int>> LiveReservationCount(Guid[] productIds);

    /// <summary>
    /// 查询所有待通知的预约数据
    /// </summary>
    /// <returns></returns>
    Task<DataTable> QueryAllReservation();

    /// <summary>
    /// 按租户更新预约为已通知状态
    /// </summary>
    /// <param name="tenantId"></param>
    /// <param name="ids"></param>
    Task<int> UpdateNotified(Guid tenantId, List<Guid> ids);
}