using HaoKao.CouponService.Domain.Models;
using HaoKao.CouponService.Domain.Repositories;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;
using Girvs.BusinessBasis.Queries;
using Girvs.Extensions.Collections;
using Girvs.Infrastructure;
using System.Data;
using HaoKao.CouponService.Domain.Enumerations;
using MySqlConnector;
using Girvs.Extensions;

namespace HaoKao.CouponService.Infrastructure.Repositories;

public class UserCouponRepository(CouponDbContext dbContext) : Repository<UserCoupon>, IUserCouponRepository
{
    /// <summary>
    /// 查询未被锁定未被使用的优惠券
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Task<List<UserCoupon>> GetUserCouponList(ProductType type)
    {
        List<ScopeEnum> list = [ScopeEnum.All, ScopeEnum.custom, type == ProductType.QuestionBlank ? ScopeEnum.QuestionBank : ScopeEnum.Course];
        var userId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();
        return Queryable.Include(navigationPropertyPath: p => p.Coupon)
                        .Where(x => x.CreatorId == userId
                                 && x.IsUse == false
                                 && x.IsLock == false
                                 && x.EndTime >= DateTime.Now
                                 && x.BeginTime <= DateTime.Now
                                 && list.Contains(x.Coupon.Scope))
                        .ToListAsync();
    }

    /// <summary>
    ///生成业绩(批量更新使用)
    /// </summary>
    /// <returns></returns>
    public Task<List<UserCoupon>> GetUserCouponListForBatch()
    {
        return Queryable.Include(p => p.Coupon).Where(x => x.IsUse == true).ToListAsync();
    }

    /// <summary>
    /// 下订单锁定过滤使用(批量)
    /// </summary>
    /// <param name="couponIds"></param>
    /// <returns></returns>
    public Task<List<UserCoupon>> GetUserCouponListForOrderLock(string couponIds)
    {
        var userId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();
        return Queryable.Include(p => p.Coupon)
                        .Where(x => couponIds.Contains(x.CouponId.ToString())
                                 && x.CreatorId == userId
                                 && x.IsUse == false
                                 && x.EndTime >= DateTime.Now
                                 && x.BeginTime <= DateTime.Now)
                        .ToListAsync();
    }

    /// <summary>
    /// 提交订单时候验证优惠券有没有被锁定（主要是用于处理同时提交订单的并发验证方式）
    /// </summary>
    /// <param name="couponIds"></param>
    /// <returns></returns>
    public Task<int> GetUserCouponCountForOrderLock(string couponIds)
    {
        var userId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();
        return Queryable.Include(p => p.Coupon)
                        .Where(x => couponIds.Contains(x.CouponId.ToString())
                                 && x.CreatorId == userId
                                 && x.IsUse == false
                                 && x.IsLock == true)
                        .CountAsync();
    }

    /// <summary>
    /// 根据订单id找到对应的优惠券id
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    public Task<List<UserCoupon>> GetUserCouponByOrderId(Guid orderId)
    {
        return Queryable.Where(x => x.OrderId == orderId).ToListAsync();
    }

    /// <summary>
    /// 后台订单详情根据(关联查询优惠券的信息)
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="productId"></param>
    /// <returns></returns>
    public Task<List<UserCoupon>> GetUserCouponByOrderIdAndProductId(Guid orderId, string productId)
    {
        return Queryable.Include(p => p.Coupon)
                        .Where(x => x.OrderId == orderId
                                 && productId.Contains(x.ProductId.ToString()))
                        .ToListAsync();
    }

    /// <summary>
    /// 读取用户优惠券
    /// </summary>
    /// <returns></returns>
    public override async Task<List<UserCoupon>> GetByQueryAsync(QueryBase<UserCoupon> query)
    {
        query.RecordCount = await Queryable.Where(query.GetQueryWhere()).CountAsync();

        if (query.RecordCount < 1)
        {
            query.Result = [];
        }
        else
        {
            query.Result = await Queryable.Include(x => x.Coupon)
                                          .Where(query.GetQueryWhere())
                                          .SelectProperties(query.QueryFields)
                                          .OrderByDescending(x => x.CreateTime)
                                          .Skip(query.PageStart)
                                          .Take(query.PageSize).ToListAsync();
        }

        return query.Result;
    }

    /// <summary>
    /// 查询所有过期前2小时的数据
    /// </summary>
    /// <returns></returns>
    public async Task<DataTable> QueryAllExpiredUserCoupon()
    {
        var database = dbContext.Database;
        var connection = database.GetDbConnection();
        var command = connection.CreateCommand();

        command.AddParameter("SchemaName", connection.Database);

        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "Sp_QueryAllExpiredUserCoupon";

        var data = new DataTable();
        await database.OpenConnectionAsync();
        var adapter = new MySqlDataAdapter((MySqlCommand)command);
        adapter.Fill(data);
        await database.CloseConnectionAsync();
        return data;
    }

    /// <summary>
    /// 按租户更新预约为已通知状态
    /// </summary>
    /// <param name="tenantId"></param>
    /// <param name="ids"></param>
    public Task<int> UpdateNotified(Guid tenantId, IEnumerable<Guid> ids)
    {
        EngineContext.Current.ClaimManager.SetFromDictionary(new Dictionary<string, string>
        {
            { GirvsIdentityClaimTypes.TenantId, tenantId.ToString() },
        });

        var tableName = EngineContext.Current.GetEntityShardingTableParameter<UserCoupon>().GetCurrentShardingTableName();

        var sql = $"UPDATE {tableName} SET Notified = true WHERE Id IN ({string.Join(",", ids.Select(x => $"'{x}'"))})";

        return dbContext.Database.ExecuteSqlRawAsync(sql);
    }

    public async Task<List<string>> GetTenantIds()
    {
        var start = $"{nameof(UserCoupon)}_".Length;
        var tables = await dbContext.GetTableNameList(nameof(UserCoupon));
        return tables.Select(t => t[start..].Insert(8, "-").Insert(13, "-").Insert(18, "-").Insert(23, "-")).ToList();
    }

    /// <summary>
    /// (存在无效优惠卷返回true，不存在无效优惠卷，返回false)
    /// </summary>
    /// <param name="couponIds"></param>
    /// <returns></returns>
    public async Task<bool> IsExistDisableCoupon(Guid[] couponIds)
    {
        var hashSet = couponIds.ToHashSet();
        var userId = EngineContext.Current.ClaimManager.GetUserId().ToGuid();
        var count = await Queryable.CountAsync(x => hashSet.Contains(x.CouponId)
                                                 && x.CreatorId == userId
                                                 && x.BeginTime <= DateTime.Now
                                                 && x.EndTime >= DateTime.Now
                                                 && x.IsLock == false
                                                 && x.IsUse == false);
        return hashSet.Count > count;
    }
}