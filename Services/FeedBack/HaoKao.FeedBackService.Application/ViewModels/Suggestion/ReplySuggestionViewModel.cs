using HaoKao.FeedBackService.Domain.Commands.Suggestion;

namespace HaoKao.FeedBackService.Application.ViewModels.Suggestion;

[AutoMapTo(typeof(ReplySuggestionCommand))]
public class ReplySuggestionViewModel : IDto
{
    /// <summary>
    /// Id
    /// </summary>
    [DisplayName("Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid Id { get; set; }

    /// <summary>
    /// 回复内容
    /// </summary>
    [DisplayName("回复内容")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string ReplyContent { get; set; }

    /// <summary>
    /// 回复截图
    /// </summary>
    [DisplayName("回复截图")]
    public List<string> ReplyScreenshots { get; set; }
}