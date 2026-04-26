namespace HaoKao.LiveBroadcastService.Application.ViewModels.SensitiveWord;

[AutoMapFrom(typeof(Domain.Entities.SensitiveWord))]
public class BrowseSensitiveWordViewModel : IDto
{
    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; }
}