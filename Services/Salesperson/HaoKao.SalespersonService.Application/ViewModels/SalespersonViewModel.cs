using HaoKao.SalespersonService.Domain.Commands;
using HaoKao.SalespersonService.Domain.Entities;
using HaoKao.SalespersonService.Domain.Queries;

namespace HaoKao.SalespersonService.Application.ViewModels;

[AutoMapTo(typeof(CreateSalespersonCommand))]
public record CreateSalespersonViewModel : IDto
{
    /// <summary>
    /// 真实姓名
    /// </summary>
    [DisplayName("真实姓名")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(100, ErrorMessage = "{0}长度不能大于{1}")]
    public string RealName { get; init; }

    /// <summary>
    /// 手机号
    /// </summary>
    [DisplayName("手机号")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(100, ErrorMessage = "{0}长度不能大于{1}")]
    public string Phone { get; init; }

    /// <summary>
    /// 企业微信用户Id
    /// </summary>
    [DisplayName("企业微信用户Id")]
    [MaxLength(100, ErrorMessage = "{0}长度不能大于{1}")]
    public string EnterpriseWeChatUserId { get; init; }

    /// <summary>
    /// 企业微信昵称
    /// </summary>
    [DisplayName("企业微信昵称")]
    [MaxLength(100, ErrorMessage = "{0}长度不能大于{1}")]
    public string EnterpriseWeChatUserName => RealName;

    /// <summary>
    /// 企业微信配置Id
    /// </summary>
    [DisplayName("企业微信配置Id")]
    public Guid EnterpriseWeChatConfigId { get; set; }
}

[AutoMapTo(typeof(UpdateSalespersonCommand))]
public record UpdateSalespersonViewModel : CreateSalespersonViewModel
{
    /// <summary>
    /// 销售人员Id
    /// </summary>
    [DisplayName("销售人员Id")]
    public Guid Id { get; init; }
}

[AutoMapTo(typeof(Salesperson))]
[AutoMapFrom(typeof(Salesperson))]
public record BrowseSalespersonViewModel : IDto
{
    /// <summary>
    /// 销售人员Id
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 真实姓名
    /// </summary>
    public string RealName { get; init; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; init; }

    /// <summary>
    /// 企业微信昵称
    /// </summary>
    public string EnterpriseWeChatUserName { get; init; }

    /// <summary>
    /// 企业微信用户Id
    /// </summary>
    public string EnterpriseWeChatUserId { get; init; }

    /// <summary>
    /// 企业微信配置Id
    /// </summary>
    public Guid EnterpriseWeChatConfigId { get; init; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; init; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime UpdateTime { get; init; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; init; }

    /// <summary>
    /// 企业微信配置
    /// </summary>
    public BrowseEnterpriseWeChatConfigViewModel EnterpriseWeChatConfig { get; init; }
}

[AutoMapTo(typeof(SalespersonQuery))]
[AutoMapFrom(typeof(SalespersonQuery))]
public class QuerySalespersonViewModel : QueryDtoBase<BrowseSalespersonViewModel>
{
    /// <summary>
    /// 真实姓名
    /// </summary>
    public string RealName { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }
}