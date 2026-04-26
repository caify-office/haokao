using HaoKao.FeedBackService.Domain.Commands.Suggestion;
using HaoKao.FeedBackService.Domain.Entities;

namespace HaoKao.FeedBackService.Application.ViewModels.Suggestion;

[AutoMapTo(typeof(CreateSuggestionCommand))]
public class CreateSuggestionViewModel : IDto
{
    /// <summary>
    /// 反馈类型
    /// </summary>
    [DisplayName("反馈类型")]
    public SuggestionType SuggestionType { get; set; }

    /// <summary>
    /// 反馈来源
    /// </summary>
    [DisplayName("反馈来源")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string SuggestionFrom { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [DisplayName("手机号")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string Phone { get; set; }

    /// <summary>
    /// 问题描述
    /// </summary>
    [DisplayName("问题描述")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string Description { get; set; }

    /// <summary>
    /// 相关截图
    /// </summary>
    [DisplayName("相关截图")]
    public List<string> Screenshots { get; set; } = [];
}