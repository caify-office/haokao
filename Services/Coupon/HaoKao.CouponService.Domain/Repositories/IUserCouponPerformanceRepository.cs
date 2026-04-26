using HaoKao.CouponService.Domain.Models;
using HaoKao.CouponService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HaoKao.CouponService.Domain.Repositories;

public interface IUserCouponPerformanceRepository : IRepository<UserCouponPerformance>
{
    /// <summary>
    /// 销售人员统计
    /// </summary>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="tenantIds"></param>
    /// <returns></returns>
    Task<(int Count, List<SellerSalesStatistics> Data)> GetSalesPerformanceStat(string startTime, string endTime, int pageIndex, int pageSize, params Guid[] tenantIds);
}