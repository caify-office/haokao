using HaoKao.PaperTempleteService.Domain.Entities;

namespace HaoKao.PaperTempleteService.Application.ViewModels;

[AutoMapFrom(typeof(PaperTemplete))]
public record BrowsePaperTempleteViewModel : IDto
{
    /// <summary>
    /// 试卷模板名称
    /// </summary>
    public string TempleteStructDatas { get; init; }
}