using HaoKao.AnsweringQuestionService.Application.ViewModels.AnsweringQuestionReply;
using HaoKao.AnsweringQuestionService.Domain.Enumerations;

namespace HaoKao.AnsweringQuestionService.Application.ViewModels.AnsweringQuestion;


[AutoMapFrom(typeof(Domain.Entities.AnsweringQuestion))]
public class BrowseAnsweringQuestionViewModel : IDto
{

    /// <summary>
    /// 手机号码
    /// </summary>

    public string Phone { get; set; }
    /// <summary>
    /// 科目名称
    /// </summary>

    public string SubjectName { get; set; }

    /// <summary>
    /// 提交时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public Guid CreatorId{ get; set; }

    /// <summary>
    /// 提交人名称
    /// </summary>
    public string UserName { get; set; }

    public Guid ParentId { get; set; }

    public Guid Id { get; set; }

    /// <summary>
    /// 是否回答
    /// </summary>
    public bool IsReply{ get; set; }

    /// <summary>
    /// 课程id
    /// </summary>
    public Guid CourseId{ get; set; }
    /// <summary>
    /// 课程名称
    /// </summary>
    public string CourseName { get; set; }
    /// <summary>
    /// 选择的课程章节id
    /// </summary>
    public Guid CourseChapterId{ get; set; }
    /// <summary>
    /// 课程章节名称
    /// </summary>
    public string CourseChapterName { get; set; }
    /// <summary>
    /// 选择的课程视频id
    /// </summary>
    public Guid CourseVideId{ get; set; }
    /// <summary>
    /// 课程视频名称
    /// </summary>
    public string CourseVideName { get; set; }
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


    //回复列表
    public List<BrowseAnsweringQuestionReplyViewModel> AnsweringQuestionReplys { get; set; }
    //追问
    public List<BrowseAnsweringQuestionViewModel> ChildQuestion { get; set; }



}
