namespace HaoKao.DrawPrizeService.Domain.Enums;

/// <summary>
/// 中奖范围
/// </summary>
public enum WinningRange
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
    /// <summary>
    /// 指定学员
    /// </summary>
    DesignatedStudent = 3,

}