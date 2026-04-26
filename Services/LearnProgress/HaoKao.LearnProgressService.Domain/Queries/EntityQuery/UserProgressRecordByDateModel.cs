using System;


namespace HaoKao.LearnProgressService.Domain.Queries.EntityQuery;

public class UserProgressRecordByDateModel
{
    /// <summary>
    /// 章节id
    /// </summary>
    public Guid ChapterId { get; set; }
    /// <summary>
    /// 主键id
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// 视频id
    /// </summary>
    public Guid VideoId { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
    /// <summary>
    /// 学习进度
    /// </summary>
    public int Progress { get; set; }
    /// <summary>
    /// 总的进度
    /// </summary>
    public int TotalProgress { get; set; }
    /// <summary>
    /// 课程id
    /// </summary>
    public Guid CourseId { get; set; }
    /// <summary>
    /// 年月日 日期
    /// </summary>
    public string DATE { get; set; }
    /// <summary>
    /// 排序日期
    /// </summary>

    public string SortDate { get; set; }
    /// <summary>
    /// 时分 显示日期
    /// </summary>
         
    public string PlayDate { get; set; }
    /// <summary>
    /// 是否结束
    /// </summary>

    public bool IsEnd { get; set; }
}