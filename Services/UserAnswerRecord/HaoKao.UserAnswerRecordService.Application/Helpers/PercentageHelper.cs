namespace HaoKao.UserAnswerRecordService.Application.Helpers;

/// <summary>
/// 百分比计算辅助类
/// </summary>
public static class PercentageHelper
{
    /// <summary>
    /// 计算百分比
    /// </summary>
    /// <param name="numerator">分子</param>
    /// <param name="denominator">分母</param>
    /// <returns>百分比整数值（0-100）</returns>
    public static int CalculatePercentage(int numerator, int denominator)
    {
        return denominator == 0 ? 0 : numerator * 100 / denominator;
    }
}