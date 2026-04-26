namespace HaoKao.LiveBroadcastService.Application.ViewModels.SensitiveWord;

[AutoMapTo(typeof(CreateSensitiveWordCommand))]
public class CreateSensitiveWordViewModel : IDto
{
    /// <summary>
    /// 内容
    /// </summary>
    [DisplayName("内容")]
    public string Content { get; set; }
}