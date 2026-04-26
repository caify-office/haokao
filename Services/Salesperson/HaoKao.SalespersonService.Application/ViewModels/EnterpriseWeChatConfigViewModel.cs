using HaoKao.SalespersonService.Domain.Commands;
using HaoKao.SalespersonService.Domain.Entities;
using HaoKao.SalespersonService.Domain.Queries;

namespace HaoKao.SalespersonService.Application.ViewModels;

[AutoMapTo(typeof(CreateEnterpriseWeChatConfigCommand))]
public record CreateEnterpriseWeChatConfigViewModel : IDto
{
    /// <summary>
    /// 企业Id
    /// </summary>
    [DisplayName("企业Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(100, ErrorMessage = "{0}长度不能大于{1}")]
    public string CorpId { get; init; }

    /// <summary>
    /// 企业名称
    /// </summary>
    [DisplayName("企业名称")]
    [MaxLength(100, ErrorMessage = "{0}长度不能大于{1}")]
    public string CorpName => EngineContext.Current.ClaimManager.GetTenantName();

    /// <summary>
    /// 应用的凭证密钥
    /// </summary>
    [DisplayName("应用的凭证密钥")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(100, ErrorMessage = "{0}长度不能大于{1}")]
    public string CorpSecret { get; init; }
}

[AutoMapTo(typeof(UpdateEnterpriseWeChatConfigCommand))]
public record UpdateEnterpriseWeChatConfigViewModel : CreateEnterpriseWeChatConfigViewModel
{
    /// <summary>
    /// 销售人员Id
    /// </summary>
    [DisplayName("销售人员Id")]
    public Guid Id { get; init; }
}

[AutoMapTo(typeof(EnterpriseWeChatConfig))]
[AutoMapFrom(typeof(EnterpriseWeChatConfig))]
public record BrowseEnterpriseWeChatConfigViewModel : IDto
{
    /// <summary>
    /// 企业微信配置Id
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 企业Id
    /// </summary>
    public string CorpId { get; init; }

    /// <summary>
    /// 企业名称
    /// </summary>
    public string CorpName { get; init; }

    /// <summary>
    /// 应用的凭证密钥
    /// </summary>
    public string CorpSecret { get; init; }
}

[AutoMapTo(typeof(EnterpriseWeChatConfigQuery))]
[AutoMapFrom(typeof(EnterpriseWeChatConfigQuery))]
public class QueryEnterpriseWeChatConfigViewModel : QueryDtoBase<BrowseEnterpriseWeChatConfigViewModel>;