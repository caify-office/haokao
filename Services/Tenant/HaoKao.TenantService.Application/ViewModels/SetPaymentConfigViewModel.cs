using HaoKao.TenantService.Domain.Entities;
using System.Collections.Generic;

namespace HaoKao.TenantService.Application.ViewModels;

/// <summary>
/// 添加考试视图模型
/// </summary>
[AutoMapFrom(typeof(Tenant))]
[AutoMapTo(typeof(Tenant))]
public class SetPaymentConfigViewModel : IDto
{
    /// <summary>
    /// 考试ID
    /// </summary>
    [DisplayName("考试ID")]
    [Required(ErrorMessage = "{0}不能为空")]

    public Guid Id { get; set; }

    /// <summary>
    /// 收款配置
    /// </summary>
    public List<PaymentConfig> PaymentConfigs { get; set; }
}