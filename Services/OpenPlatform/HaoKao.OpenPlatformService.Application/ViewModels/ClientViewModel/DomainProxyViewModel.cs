namespace HaoKao.OpenPlatformService.Application.ViewModels.ClientViewModel;

/// <summary>
/// 域名代理设置
/// </summary>
public class DomainProxyViewModel : IDto
{
    /// <summary>
    /// 域名
    /// </summary>  
    [DisplayName("域名")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(300, ErrorMessage = "{0}长度不能大于{1}")]
    public string Domain { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    [DisplayName("租户Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid TenantId { get; set; }

    /// <summary>
    /// 租户名称
    /// </summary>
    [DisplayName("租户名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(500, ErrorMessage = "{0}长度不能大于{1}")]
    public string TenantName { get; set; }

}