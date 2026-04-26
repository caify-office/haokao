namespace HaoKao.OrderService.Domain.Enums;

/// <summary>
/// 发票类型(企业发票/个人发票)
/// </summary>
public enum InvoiceType
{
    /// <summary>
    /// 企业发票
    /// </summary>
    Business,

    /// <summary>
    /// 个人发票
    /// </summary>
    Personal,
}

/// <summary>
/// 增值票类型(普通发票/专用发票）
/// </summary>
public enum VatInvoiceType
{
    /// <summary>
    /// 增值税普通发票
    /// </summary>
    General,

    /// <summary>
    /// 增值税专项发票
    /// </summary>
    Special,
}

/// <summary>
/// 发票形式(电子发票/纸质发票)
/// </summary>
public enum InvoiceFormat
{
    /// <summary>
    /// 电子发票
    /// </summary>
    Electronic,

    /// <summary>
    /// 纸质发票
    /// </summary>
    Paper,
}

/// <summary>
/// 申请状态(待开票/已开票)
/// </summary>
public enum RequestState
{
    /// <summary>
    /// 待开票
    /// </summary>
    Waiting,

    /// <summary>
    /// 已开票
    /// </summary>
    Success,
}