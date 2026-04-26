using System;
using System.ComponentModel;

namespace HaoKao.SubjectService.Domain.SubjectModule;

/// <summary>
/// 科目
/// </summary>
public class Subject : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeMultiTenant<Guid>
{
    /// <summary>
    /// 命审题科目Id
    /// </summary>
    public string TrialSubjectId { get; set; }

    /// <summary>
    /// 科目名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 普通科目/专业科目
    /// </summary>
    public SubjectTypeEnum IsCommon { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 是否显示
    /// </summary>
    public bool IsShow { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}

/// <summary>
/// 科目类型
/// </summary>
[Description("科目类型")]
public enum SubjectTypeEnum
{
    /// <summary>
    /// 未知
    /// </summary>
    [Description("普通科目")]
    Common = 0,

    /// <summary>
    /// 未发布
    /// </summary>
    [Description("专业科目")]
    Subject = 1,
}