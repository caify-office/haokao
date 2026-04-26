using System;
using HaoKao.QuestionCategoryService.Domain.Enums;

namespace HaoKao.QuestionCategoryService.Domain.Entities;

/// <summary>
/// 题库类别
/// </summary>
public class QuestionCategory : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeMultiTenant<Guid>, ITenantShardingTable
{
    /// <summary>
    /// 类名名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 适应场景
    /// </summary>
    public AdaptPlace AdaptPlace { get; set; }

    /// <summary>
    /// 类别代码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 显示条件
    /// </summary>
    public DisplayConditionEnum DisplayCondition { get; set; }

    /// <summary>
    /// 产品包Id(购买跳转对象)
    /// </summary>
    public Guid ProductPackageId { get; set; }

    /// <summary>
    /// 产品包类型
    /// </summary>
    public ProductPackageType? ProductPackageType { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}