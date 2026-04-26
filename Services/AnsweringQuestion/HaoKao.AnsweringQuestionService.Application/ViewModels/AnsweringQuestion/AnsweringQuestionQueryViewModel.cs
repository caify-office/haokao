using HaoKao.AnsweringQuestionService.Domain.Enumerations;
using HaoKao.AnsweringQuestionService.Domain.Queries.EntityQuery;

namespace HaoKao.AnsweringQuestionService.Application.ViewModels.AnsweringQuestion;


[AutoMapFrom(typeof(AnsweringQuestionQuery))]
[AutoMapTo(typeof(AnsweringQuestionQuery))]
public class AnsweringQuestionQueryViewModel: QueryDtoBase<AnsweringQuestionQueryListViewModel>
{

    /// <summary>
    /// 科目
    /// </summary>
    public Guid? SubjectId { get; set; }

    /// <summary>
    /// 提交人
    /// </summary>
    public string Submitor { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 是否回复
    /// </summary>
    public bool? IsReply { get; set; }

    /// <summary>
    /// 是否新回复
    /// </summary>
    public bool? IsNewReply { get; set; }
    /// <summary>
    /// 开始时间
    /// </summary>
    public string StartTime { get; set; }
    /// <summary>
    /// 结束时间
    /// </summary>
    public string EndTime { get; set; }
    /// <summary>
    /// 分类
    /// </summary>
    public AnsweringQuestionEnum? Type { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.AnsweringQuestion))]
[AutoMapTo(typeof(Domain.Entities.AnsweringQuestion))]
public class AnsweringQuestionQueryListViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 用户名称
    /// </summary>
    public string UserName{ get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    public string Phone{ get; set; }

    /// <summary>
    /// 科目id
    /// </summary>
    public Guid SubjectId{ get; set; }

    /// <summary>
    /// 科目名称
    /// </summary>
    public string SubjectName{ get; set; }

    /// <summary>
    /// 是否回答
    /// </summary>
    public bool IsReply{ get; set; }

    /// <summary>
    /// 课程id
    /// </summary>
    public Guid CourseId{ get; set; }

    /// <summary>
    /// 选择的课程章节id
    /// </summary>
    public Guid CourseChapterId{ get; set; }

    /// <summary>
    /// 选择的课程视频id
    /// </summary>
    public Guid CourseVideId{ get; set; }

    /// <summary>
    /// 书籍页码
    /// </summary>
    public string BookPageSize{ get; set; }

    /// <summary>
    /// 书籍名称以及相关描述
    /// </summary>
    public string BookName{ get; set; }

    /// <summary>
    /// 提问类型
    /// </summary>
    public AnsweringQuestionEnum Type { get; set; }

    /// <summary>
    /// 问题描述
    /// </summary>
    public string Description{ get; set; }

    /// <summary>
    /// 详细描述
    /// </summary>
    public string Remark{ get; set; }

    /// <summary>
    /// 上传的图片路劲
    /// </summary>
    public string FileUrl{ get; set; }

    /// <summary>
    /// 观看人数累加
    /// </summary>
    public int WatchCount{ get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 创建人id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// true-新回复  false--没有新回复
    /// </summary>
    public bool IsNewRepley { get; set; }
    /// <summary>
    /// 回复人
    /// </summary>
    public string ReplyUserName { get; set; }
    /// <summary>
    /// 回复时间
    /// </summary>
    public DateTime ReplyDateTime { get; set; }


}