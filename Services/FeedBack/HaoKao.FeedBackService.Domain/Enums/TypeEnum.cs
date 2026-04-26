using System.ComponentModel;

namespace HaoKao.FeedBackService.Domain.Enums;

/// <summary>
/// 反馈类型
/// </summary>
[Description("反馈类型")]
public enum TypeEnum
{
    /// <summary>
    /// 意见发馈
    /// </summary>
    [Description("意见发馈")]
    Suggest = 1,

    /// <summary>
    /// 投诉
    /// </summary>
    [Description("投诉")]
    Complaint = 2,
  
}