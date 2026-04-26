using Girvs.Extensions;
using HaoKao.DrawPrizeService.Domain.Entities;

namespace HaoKao.DrawPrizeService.Domain.Queries;

public class DrawPrizeRecordQuery : QueryBase<DrawPrizeRecord>
{
    /// <summary>
    /// 创建者id
    /// </summary>
    [QueryCacheKey]
    public Guid? CreatorId { get; set; }
    /// <summary>
    /// 所属抽奖活动Id
    /// </summary>
    [QueryCacheKey]
    public Guid? DrawPrizeId { get; set; }
    /// <summary>
    /// 创建者名称
    /// </summary>
    [QueryCacheKey]
    public string CreatorName { get; set; }
    /// <summary>
    /// 奖品名称
    /// </summary>
    [QueryCacheKey]
    public string PrizeName { get; set; }

    /// <summary>
    /// 是否中奖（true：返回中奖记录，false：返回没中奖记录，不传：都返回）
    /// </summary>
    [QueryCacheKey]
    public bool? IsWinning { get; set; }
    public override Expression<Func<DrawPrizeRecord, bool>> GetQueryWhere()
    {
        Expression<Func<DrawPrizeRecord, bool>> expression = x => true;
        if (CreatorId.HasValue)
        {
            expression = expression.And(x => x.CreatorId == CreatorId);
        }
        if (DrawPrizeId.HasValue)
        {
            expression = expression.And(x => x.DrawPrizeId == DrawPrizeId);
        }
        if (!string.IsNullOrEmpty(CreatorName))
        {
            expression = expression.And(x => x.CreatorName.Contains(CreatorName));
        }
        if (!string.IsNullOrEmpty(PrizeName))
        {
            expression = expression.And(x => x.PrizeName.Contains(PrizeName));
        }
        if (IsWinning.HasValue)
        {
            if (IsWinning.Value)
            {
                //中奖
                expression = expression.And(x => x.PrizeId != Guid.Empty);
            }
            else
            {
                //未中奖
                expression = expression.And(x => x.PrizeId == Guid.Empty);
            }
        }
        return expression;
    }
}
