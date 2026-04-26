using System.ComponentModel;

namespace HaoKao.CouponService.Domain.Enumerations;

/// <summary>
/// 使用范围
/// </summary>
[Description("使用范围")]
public enum ScopeEnum
{
    /// <summary>
    /// 全部
    /// </summary>
    [Description("0")]
    All = 0,

    /// <summary>
    /// 题库
    /// </summary>
    [Description("1")]
    QuestionBank = 1,

    /// <summary>
    /// 课程
    /// </summary>
    [Description("2")]
    Course = 2,

    /// <summary>
    /// 自定义 选择产品包ids
    /// </summary>
    [Description("3")]
    custom = 3,
}