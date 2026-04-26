using HaoKao.TenantService.Domain.Entities;
using HaoKao.TenantService.Domain.Enums;
using HaoKao.TenantService.Domain.Queries;
using System.Collections.Generic;

namespace HaoKao.TenantService.Application.ViewModels;

[AutoMapFrom(typeof(TenantQuery))]
[AutoMapTo(typeof(TenantQuery))]
public class TenantQueryViewModel : QueryDtoBase<TenantQueryListViewModel>
{
    /// <summary>
    /// 租户名称
    /// </summary>
    public string TenantName { get; set; }

    /// <summary>
    /// 租户代码
    /// </summary>
    public string TenantNo { get; set; }

    /// <summary>
    /// 租户启用状态
    /// </summary>
    public EnableState? StartState { get; set; }

    /// <summary>
    /// 其它名称ID，一般主要是数据字典中的Id
    /// </summary>
    public Guid? OtherId { get; set; }
}

[AutoMapFrom(typeof(Tenant))]
public class TenantQueryListViewModel : IDto
{
    /// <summary>
    /// ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 其它名称ID，一般主要是数据字典中的Id
    /// </summary>
    public Guid? OtherId { get; set; }

    /// <summary>
    /// 其它名称
    /// </summary>
    public string OtherName { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string TenantName { get; set; }

    /// <summary>
    /// 代码
    /// </summary>
    public string TenantNo { get; set; }

    /// <summary>
    /// 功能模块集合
    /// </summary>
    public SystemModule SystemModule { get; set; }

    /// <summary>
    /// 年度租户时间
    /// </summary>
    public DateTime AnnualExamTime { get; set; }

    /// <summary>
    /// 发布状态
    /// </summary>
    public ReleaseState ReleaseState { get; set; }

    /// <summary>
    /// 启用状态
    /// </summary>
    public EnableState StartState { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    public string Instructions { get; set; }

    /// <summary>
    /// 管理员账户
    /// </summary>
    public string AdminUserAcount { get; set; }

    /// <summary>
    /// 管理员姓名
    /// </summary>
    public string AdminUserName { get; set; }

    /// <summary>
    /// 管理员手机号
    /// </summary>
    public string AdminPhone { get; set; }

    /// <summary>
    /// 收款配置
    /// </summary>
    public List<PaymentConfig> PaymentConfigs { get; set; }
}