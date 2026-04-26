using HaoKao.OrderService.Domain.Entities;
using HaoKao.OrderService.Domain.Enums;
using System.Linq;

namespace HaoKao.OrderService.Domain.Queries;

public class PlatformPayerQuery : QueryBase<PlatformPayer>
{
    /// <summary>
    /// 支付名称
    /// </summary>
    [QueryCacheKey]
    public string Name { get; set; }

    /// <summary>
    /// 对应支付处理者Id
    /// </summary>
    [QueryCacheKey]
    public Guid? PayerId { get; set; }

    /// <summary>
    /// 启用/禁用
    /// </summary>
    [QueryCacheKey]
    public bool? UseState { get; set; }

    /// <summary>
    /// 支付归类
    /// </summary>
    [QueryCacheKey]
    public PaymentMethod? PaymentMethod { get; set; }

    /// <summary>
    /// 支付场景
    /// </summary>
    [QueryCacheKey]
    public PlatformPayerScenes? PlatformPayerScenes { get; set; }

    /// <summary>
    /// ID列表
    /// </summary>
    [QueryCacheKey]
    public IReadOnlyList<Guid> Ids { get; set; }

    /// <summary>
    /// 苹果端是否显示
    /// </summary>
    [QueryCacheKey]
    public bool? IosIsOpen { get; set; }

    public override Expression<Func<PlatformPayer, bool>> GetQueryWhere()
    {
        Expression<Func<PlatformPayer, bool>> expression = x => true;

        if (PlatformPayerScenes.HasValue)
        {
            expression = expression.And(x => x.PlatformPayerScenes == PlatformPayerScenes);
        }

        if (Ids is { Count: > 0 })
        {
            expression = expression.And(x => Ids.Contains(x.Id));
        }

        if (IosIsOpen.HasValue)
        {
            expression = expression.And(x => x.IosIsOpen == IosIsOpen);
        }

        if (PaymentMethod.HasValue)
        {
            expression = expression.And(x => x.PaymentMethod == PaymentMethod);
        }

        if (!Name.IsNullOrEmpty())
        {
            expression = expression.And(x => x.Name.Contains(Name));
        }

        if (PayerId.HasValue)
        {
            expression = expression.And(x => x.PayerId == PayerId);
        }

        if (UseState.HasValue)
        {
            expression = expression.And(x => x.UseState == UseState);
        }

        return expression;
    }
}