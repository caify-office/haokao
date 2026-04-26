using HaoKao.AnsweringQuestionService.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaoKao.AnsweringQuestionService.Domain.Entities;

/// <summary>
/// 答疑
/// </summary>
public class AnsweringQuestion : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeMultiTenant<Guid>, ITenantShardingTable, IIncludeCreatorId<Guid>
{

    public Guid? ParentId { get; set; }
    /// <summary>
    /// 用户名称
    /// </summary>

    public string UserName { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 科目id
    /// </summary>

    public Guid SubjectId { get; set; }
    /// <summary>
    /// 科目名称
    /// </summary>

    public string  SubjectName { get; set; }

    /// <summary>
    /// 是否回答
    /// </summary>
    public bool IsReply { get; set; }
   
    /// <summary>
    /// 课程id
    /// </summary>
    public Guid CourseId { get; set; }
    /// <summary>
    /// 课程名称
    /// </summary>
    public string CourseName { get; set; }
    /// <summary>
    /// 选择的课程章节id
    /// </summary>

    public Guid CourseChapterId { get; set; }
    /// <summary>
    /// 课程章节名称
    /// </summary>
    public string CourseChapterName { get; set; }
    /// <summary>
    /// 选择的课程视频id
    /// </summary>
    public Guid CourseVideId { get; set; }
    /// <summary>
    /// 课程视频名称
    /// </summary>
    public string CourseVideName{ get; set; }

    /// <summary>
    /// 书籍页码
    /// </summary>
    public string  BookPageSize { get; set; }

    /// <summary>
    /// 书籍名称以及相关描述
    /// </summary>

    public string BookName { get; set; }
    /// <summary>
    /// 提问类型
    /// </summary>
    public AnsweringQuestionEnum Type { get; set; }
    /// <summary>
    /// 问题描述
    /// </summary>

    public string Description { get; set; }

    /// <summary>
    /// 详细描述
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// 上传的图片路劲
    /// </summary>
    public string FileUrl { get; set; }
    /// <summary>
    /// 观看人数累加
    /// </summary>
    public int WatchCount { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    public Guid TenantId { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public Guid CreatorId { get; set; }


    //回复列表
 
    public virtual List<AnsweringQuestionReply> AnsweringQuestionReplys { get; set; }
    //追问
    
    [ForeignKey("ParentId")]
    public List<AnsweringQuestion> ChildQuestion { get; set; }

    public Guid ProductId { get; set; }

}