using HaoKao.AnsweringQuestionService.Domain.Enumerations;
using HaoKao.AnsweringQuestionService.Domain.Queries.EntityQuery;

namespace HaoKao.AnsweringQuestionService.Application.ViewModels.AnsweringQuestion;


[AutoMapFrom(typeof(AnsweringQuestionWebQuery))]
[AutoMapTo(typeof(AnsweringQuestionWebQuery))]
public class AnsweringQuestionQueryWebViewModel : QueryDtoBase<AnsweringQuestionQueryListWebViewModel>
{
    public Guid? ProductId { get; set; }
    public Guid? SubjectId { get; set; }
    public Guid? UserId { get; set; }

    /// <summary>
    /// 课程id
    /// </summary>
    public Guid? CourseId { get; set; }

    /// <summary>
    /// 关键词搜索
    /// </summary>
    public string SearchKey { get; set; }

    /// <summary>
    /// 分类
    /// </summary>
    public AnsweringQuestionEnum? Type { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.AnsweringQuestion))]
[AutoMapTo(typeof(Domain.Entities.AnsweringQuestion))]
public class AnsweringQuestionQueryListWebViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 是否回答
    /// </summary>
    public bool IsReply{ get; set; }

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


}