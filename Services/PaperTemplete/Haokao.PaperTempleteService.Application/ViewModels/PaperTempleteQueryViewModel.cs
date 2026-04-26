using HaoKao.PaperTempleteService.Domain.Entities;
using HaoKao.PaperTempleteService.Domain.Queries;

namespace HaoKao.PaperTempleteService.Application.ViewModels;

[AutoMapFrom(typeof(PaperTempleteQuery))]
[AutoMapTo(typeof(PaperTempleteQuery))]
public class PaperTempleteQueryViewModel : QueryDtoBase<PaperTempleteQueryListViewModel>
{
    /// <summary>
    /// 试卷模板名称
    /// </summary>
    public string TempleteName { get; set; }
}

[AutoMapFrom(typeof(PaperTemplete))]
[AutoMapTo(typeof(PaperTemplete))]
public record PaperTempleteQueryListViewModel : IDto
{
    /// <summary>
    /// 试卷模板Id
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 试卷模板名称
    /// </summary>
    public string TempleteName { get; init; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; init; }

    /// <summary>
    /// 模板适用科目
    /// </summary>
    public string SuitableSubjects { get; init; }
}