using System.Collections.Generic;

namespace HaoKao.LecturerService.Domain.Entities;

/// <summary>
/// 讲师
/// </summary>
public class Lecturer : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeUpdateTime, IIncludeMultiTenant<Guid>, ITenantShardingTable
{
    /// <summary>
    /// 教师姓名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 科目Id数组
    /// </summary>
    public List<Guid> SubjectIds { get; set; }

    /// <summary>
    /// 科目名称数组
    /// </summary>
    public List<string> SubjectNames { get; set; }

    /// <summary>
    /// 简介
    /// </summary>
    public string Desc { get; set; }

    /// <summary>
    /// 课程介绍(前端已废弃不再使用该字段)
    /// </summary>
    public string CourseIntroduction { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string HeadPortraitUrl { get; set; }

    /// <summary>
    /// 形象照
    /// </summary>
    public string PhotoUrl { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 关联的产品包id
    /// </summary>
    public List<Guid> ProductPackageIds { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}