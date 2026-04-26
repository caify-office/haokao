namespace HaoKao.LiveBroadcastService.Application.ViewModels.SensitiveWord;

[AutoMapTo(typeof(UpdateSensitiveWordCommand))]
public class UpdateSensitiveWordViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    [DisplayName("内容")]
    public string Content { get; set; }
}