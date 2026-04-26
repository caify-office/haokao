namespace HaoKao.LiveBroadcastService.Application.ViewModels.SensitiveWord;

[AutoMapFrom(typeof(SensitiveWordQuery))]
[AutoMapTo(typeof(SensitiveWordQuery))]
public class SensitiveWordQueryViewModel : QueryDtoBase<SensitiveWordQueryListViewModel>
{
    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.SensitiveWord))]
[AutoMapTo(typeof(Domain.Entities.SensitiveWord))]
public class SensitiveWordQueryListViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; }
}