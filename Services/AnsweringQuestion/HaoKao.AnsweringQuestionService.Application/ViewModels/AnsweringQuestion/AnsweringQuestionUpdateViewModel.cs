using System.ComponentModel;

namespace HaoKao.AnsweringQuestionService.Application.ViewModels.AnsweringQuestion;


[AutoMapTo(typeof(Domain.Entities.AnsweringQuestion))]
public class UpdateAnsweringQuestionViewModel : IDto
{

    /// <summary>
    /// 观看人数累加
    /// </summary>
    [DisplayName("观看人数累加")]
    public int WatchCount{ get; set; }
}