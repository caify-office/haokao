using Girvs.BusinessBasis.Queries;
using Girvs.Extensions.Collections;
using HaoKao.CouponService.Domain.Models;
using HaoKao.CouponService.Domain.Repositories;
using HaoKao.CouponService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaoKao.CouponService.Infrastructure.Repositories;

public class UserCouponPerformanceRepository(CouponDbContext dbContext) : Repository<UserCouponPerformance>, IUserCouponPerformanceRepository
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
    public async Task<(int Count, List<SellerSalesStatistics> Data)> GetSalesPerformanceStat(
        string startTime,
        string endTime,
        int pageIndex,
        int pageSize,
        params Guid[] tenantIds
    )
    {
        var where = new StringBuilder();
        where.Append("WHERE 1 = 1");
        if (!string.IsNullOrEmpty(startTime))
        {
            where.Append($" AND PayTime >= '{startTime}'");
        }
        if (!string.IsNullOrEmpty(endTime))
        {
            where.Append($" AND PayTime <= '{endTime}'");
        }

        // 检查租户分表是否存在
        var existTables = await dbContext.GetTableNameList(nameof(UserCouponPerformance));
        var tableNames = existTables.Where(x => tenantIds.Any(t => x.Contains(t.ToString().Replace("-", "")))).ToList();

        if (tableNames.Count == 0)
        {
            return (0, []);
        }

        var unionSql = string.Join("\n\tUNION ALL\n", tableNames.Select(t => $"\tSELECT PersonName, COUNT(0) AS TotalCount, SUM(`FactAmount`) AS TotalAmount FROM {t} {where} GROUP BY PersonName"));
        var querySql = $"SELECT t.PersonName, SUM(t.TotalCount) TotalCount, SUM(t.TotalAmount) TotalAmount FROM (\n{unionSql}\n) t GROUP BY t.PersonName";

        var query = dbContext.Database.SqlQueryRaw<SellerSalesStatistics>(querySql);

        var count = await query.CountAsync();
        if (count > 0)
        {
            var data = await query.OrderByDescending(x => x.TotalAmount)
                                  .ThenByDescending(x => x.TotalCount)
                                  .Skip((pageIndex - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();
            return (count, data);
        }

        return (0, []);
    }

    /// <summary>
    /// 业绩管理
    /// </summary>
    /// <returns></returns>
    public override async Task<List<UserCouponPerformance>> GetByQueryAsync(QueryBase<UserCouponPerformance> query)
    {
        query.RecordCount = await Queryable.Where(query.GetQueryWhere()).CountAsync();

        if (query.RecordCount < 1)
        {
            query.Result = [];
        }
        else
        {
            query.Result = await Queryable.Where(query.GetQueryWhere())
                                          .SelectProperties(query.QueryFields)
                                          .OrderByDescending(x => x.PayTime)
                                          .Skip(query.PageStart)
                                          .Take(query.PageSize).ToListAsync();
        }

        return query.Result;
    }
}