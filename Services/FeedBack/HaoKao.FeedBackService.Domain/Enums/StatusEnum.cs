using System.ComponentModel;

namespace HaoKao.FeedBackService.Domain.Enums;

/// <summary>
/// 处理状态--已提交、已回复、已完结
/// </summary>
[Description("处理状态")]
public enum StatusEnum
{
    /// <summary>
    /// 已提交
    /// </summary>
    [Description("已提交")]
    Submit = 1,

    /// <summary>
    /// 已回复
    /// </summary>
    [Description("已处理")]
    NoHandle = 2,

    /// <summary>
    /// 已完结
    /// </summary>
    [Description("已完结")]
    Finished = 3,


}