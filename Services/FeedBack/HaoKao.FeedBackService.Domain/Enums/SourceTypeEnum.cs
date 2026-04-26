using System.ComponentModel;

namespace HaoKao.FeedBackService.Domain.Enums;

/// <summary>
/// 反馈来源
/// </summary>
[Description("反馈来源")]
public enum SourceTypeEnum
{
    /// <summary>
    /// Web
    /// </summary>
    [Description("Web")]
    Web = 1,

    /// <summary>
    /// App
    /// </summary>
    [Description("App")]
    App = 1,

    /// <summary>
    /// 小程序
    /// </summary>
    [Description("小程序")]
    WeChat = 2,
}