namespace HaoKao.AnsweringQuestionService.Application.ViewModels.AnsweringQuestionReply;


[AutoMapFrom(typeof(Domain.Entities.AnsweringQuestionReply))]
public class BrowseAnsweringQuestionReplyViewModel : IDto
{

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
