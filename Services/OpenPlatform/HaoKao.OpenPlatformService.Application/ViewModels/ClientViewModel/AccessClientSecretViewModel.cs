using HaoKao.OpenPlatformService.Domain.Entities;

namespace HaoKao.OpenPlatformService.Application.ViewModels.ClientViewModel;
[AutoMapFrom(typeof(AccessClientSecret))]
public class AccessClientSecretViewModel :IDto
{
    /// <summary>
    /// 描述
    /// </summary>
    [DisplayName("描述")]
    [MaxLength(2000, ErrorMessage = "{0}长度不能大于{1}")]
    public string Description { get; set; }
    /// <summary>
    /// 密钥值 
    /// </summary>
    [DisplayName("密钥值")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(4000, ErrorMessage = "{0}长度不能大于{1}")]
    public string Value { get; set; }
    /// <summary>
    /// 到期
    /// </summary>
    [DisplayName("到期")]
    public DateTime? Expiration { get; set; }
    /// <summary>
    /// 密钥类型
    /// </summary>
    [DisplayName("密钥类型")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(250, ErrorMessage = "{0}长度不能大于{1}")]
    public string Type { get; set; } = "SharedSecret";

    /// <summary>
    ///哈希类型 
    /// </summary>
    [DisplayName("哈希类型 ")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(250, ErrorMessage = "{0}长度不能大于{1}")]
    public string HashType { get; set; } = "Sha256";

    [DisplayName("创建时间")]
    public DateTime Created { get; set; } = DateTime.UtcNow;


  
}