using HaoKao.TenantService.Domain.Entities;
using HaoKao.TenantService.Domain.Enums;

namespace HaoKao.TenantService.Application.ViewModels;

/// <summary>
/// 添加租户视图模型
/// </summary>
[AutoMapFrom(typeof(Tenant))]
[AutoMapTo(typeof(Tenant))]
public class UpdateTenantViewModel : CreateTenantViewModel
{
    /// <summary>
    /// 租户ID
    /// </summary>
    [DisplayName("租户ID")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid Id { get; set; }
}

/// <summary>
/// 启用/禁用视图模型
/// </summary>
[AutoMapFrom(typeof(Tenant))]
[AutoMapTo(typeof(Tenant))]
public class EnableDisabledTenantViewModel : IDto
{
    /// <summary>
    /// ID
    /// </summary>
    [DisplayName("ID")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid Id { get; set; }

    /// <summary>
    /// 启用/禁用状态
    /// </summary>
    [DisplayName("启用/禁用状态")]
    [Required(ErrorMessage = "{0}不能为空")]
    public EnableState StartState { get; set; }
}