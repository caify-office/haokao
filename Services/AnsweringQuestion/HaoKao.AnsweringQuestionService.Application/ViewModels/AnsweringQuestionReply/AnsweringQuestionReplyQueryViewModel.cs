using HaoKao.AnsweringQuestionService.Domain.Queries.EntityQuery;

namespace HaoKao.AnsweringQuestionService.Application.ViewModels.AnsweringQuestionReply;


[AutoMapFrom(typeof(AnsweringQuestionReplyQuery))]
[AutoMapTo(typeof(AnsweringQuestionReplyQuery))]
public class AnsweringQuestionReplyQueryViewModel: QueryDtoBase<AnsweringQuestionReplyQueryListViewModel>;

[AutoMapFrom(typeof(Domain.Entities.AnsweringQuestionReply))]
[AutoMapTo(typeof(Domain.Entities.AnsweringQuestionReply))]
public class AnsweringQuestionReplyQueryListViewModel : IDto
{
     public Guid Id { get; set; }

    /// <summary>
    /// 答疑回复内容
    /// </summary>
    public string ReplyContent{ get; set; }

    /// <summary>
    /// 回复人用户名
    /// </summary>
    public string ReplyUserName{ get; set; }

    /// <summary>
    /// 关联的题目id
    /// </summary>
    public Guid AnsweringQuestionId{ get; set; }

    public DateTime CreateTime { get; set; }
}