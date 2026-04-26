using HaoKao.CouponService.Domain.Models;
using HaoKao.CouponService.Domain.Repositories;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using Girvs.BusinessBasis.Queries;
using Girvs.Extensions.Collections;

namespace HaoKao.CouponService.Infrastructure.Repositories;

public class CouponRepository : Repository<Coupon>, ICouponRepository
{
    /// <summary>
    /// 查询详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public override Task<Coupon> GetByIdAsync(Guid id)
    {
        return Queryable.Include(x => x.UserCoupons).FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <summary>
    /// 优惠券管理列表
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public override async Task<List<Coupon>> GetByQueryAsync(QueryBase<Coupon> query)
    {
        query.RecordCount = await Queryable.Where(query.GetQueryWhere()).CountAsync();

        if (query.RecordCount < 1)
        {
            query.Result = [];
        }
        else
        {
            query.Result = await Queryable.Include(x => x.UserCoupons)
                                          .Where(query.GetQueryWhere())
                                          .SelectProperties(query.QueryFields)
                                          .OrderByDescending(x => x.CreateTime)
                                          .Skip(query.PageStart)
                                          .Take(query.PageSize).ToListAsync();
        }

        return query.Result;
    }
}