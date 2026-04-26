using HaoKao.CouponService.Domain.Models;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Data;
using HaoKao.CouponService.Domain.Enumerations;

namespace HaoKao.CouponService.Domain.Repositories;

public interface IUserCouponRepository : IRepository<UserCoupon>
{
    /// <summary>
    /// 查询当前时间可用优惠券(下单选择使用)
    /// </summary>
    /// <returns></returns>
    Task<List<UserCoupon>> GetUserCouponList(ProductType type);

    /// <summary>
    /// 根据订单id找到对应的优惠券id
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    Task<List<UserCoupon>> GetUserCouponByOrderId(Guid orderId);

    /// <summary>
    /// 后台查询订单详情使用
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="productId"></param>
    /// <returns></returns>
    Task<List<UserCoupon>> GetUserCouponByOrderIdAndProductId(Guid orderId, string productId);

    /// <summary>
    ///生成业绩(批量更新使用)
    /// </summary>
    /// <returns></returns>
    Task<List<UserCoupon>> GetUserCouponListForBatch();

    /// <summary>
    /// 下订单锁定过滤使用
    /// </summary>
    /// <param name="couponIds"></param>
    /// <returns></returns>
    Task<List<UserCoupon>> GetUserCouponListForOrderLock(string couponIds);

    /// <summary>
    /// 提交订单时候验证优惠券有没有被锁定（主要是用于处理同时提交订单的并发验证方式）
    /// </summary>
    /// <param name="couponIds"></param>
    /// <returns></returns>
    Task<int> GetUserCouponCountForOrderLock(string couponIds);

    /// <summary>
    /// 查询所有过期前2小时的数据
    /// </summary>
    /// <returns></returns>
    Task<DataTable> QueryAllExpiredUserCoupon();

    /// <summary>
    /// 按租户更新预约为已通知状态
    /// </summary>
    /// <param name="tenantId"></param>
    /// <param name="ids"></param>
    Task<int> UpdateNotified(Guid tenantId, IEnumerable<Guid> ids);

    Task<List<string>> GetTenantIds();

    Task<bool> IsExistDisableCoupon(Guid[] couponIds);
}