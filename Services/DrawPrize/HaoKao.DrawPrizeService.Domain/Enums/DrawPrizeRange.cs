namespace HaoKao.DrawPrizeService.Domain.Enums;

/// <summary>
/// 抽奖范围
/// </summary>
public enum DrawPrizeRange
{
    /// <summary>
    /// 全部
    /// </summary>
    All = 0,
    /// <summary>
    /// 付费学员
    /// </summary>
    PaidStudents = 1,
    /// <summary>
    /// 非付费学员
    /// </summary>
    NonPaidStudents = 2,

}