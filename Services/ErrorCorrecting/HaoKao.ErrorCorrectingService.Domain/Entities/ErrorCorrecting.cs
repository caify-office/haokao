using System;
using System.ComponentModel;

namespace HaoKao.ErrorCorrectingService.Domain.Entities;

/// <summary>
/// 题库类别
/// </summary>
public class ErrorCorrecting : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeMultiTenant<Guid>, ITenantShardingTable
{
    /// <summary>
    /// 问题id
    /// </summary>
    public Guid QuestionId { get; set; }

    /// <summary>
    /// 用户id
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 问题类型
    /// </summary>
    public string QuestionTypes { get; set; }

    /// <summary>
    /// 时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    public Guid TenantId { get; set; }

    public Guid SubjectId { get; set; } //科目ID

    public string SubjectName { get; set; } //科目名称

    public Guid QuestionTypeId { get; set; } //题型类别id

    public string QuestionTypeName { get; set; } //题型类别名称

    public string QuestionText { get; set; } //题干

    public string NickName { get; set; } //昵称

    public string Phone { get; set; } //手机号码

    public Guid CategoryId { get; set; } //分类id

    public string CategoryName { get; set; } //分类名称

    public int Status { get; set; }
}

/// <summary>
/// 纠错处理状态
/// </summary>
[Description("纠错处理状态")]
public enum StatusEnum
{
    /// <summary>
    /// 未处理
    /// </summary>
    [Description("未处理")]
    NoHandle = 0,

    /// <summary>
    /// 已处理
    /// </summary>
    [Description("已处理")]
    HasHandle = 1,

    /// <summary>
    /// 未知错误
    /// </summary>
    [Description("未知错误")]
    Wrong = 2,
}