using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HaoKao.AnsweringQuestionService.Application.ViewModels.AnsweringQuestionReply;


[AutoMapTo(typeof(Domain.Entities.AnsweringQuestionReply))]
public class CreateAnsweringQuestionReplyViewModel : IDto
{

    /// <summary>
    /// 答疑回复内容
    /// </summary>
    [DisplayName("答疑回复内容")]
    [Required(ErrorMessage = "{0}不能为空")]

    public string ReplyContent{ get; set; }



    /// <summary>
    /// 关联的题目id
    /// </summary>
    [DisplayName("关联的题目id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid AnsweringQuestionId{ get; set; }
}