namespace HaoKao.NotificationMessageService.Domain.Models;

/// <summary>
/// 注册用户
/// </summary>
public class RegisteredUser
{
    /// <summary>
    /// 姓名
    /// </summary>
    public string UserName { get; set; }
        
    /// <summary>
    /// 身份证号
    /// </summary>
    public string CardId { get; set; }
        
    /// <summary>
    /// 手机号
    /// </summary>
    public string ContactNumber { get; set; }
        
    /// <summary>
    /// OpenId
    /// </summary>
    public string OpenId { get; set; }
}