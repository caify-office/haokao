using HaoKao.PaperService.Domain.Commands;
using HaoKao.PaperService.Domain.Enumerations;

namespace HaoKao.PaperService.Application.ViewModels;

[AutoMapTo(typeof(UpdatePaperCommand))]
public record UpdatePaperViewModel : CreatePaperViewModel
{
    /// <summary>
    /// 主键
    /// </summary>
    public Guid Id { get; init; }
}

/// <summary>
/// 更新试卷是否限免请求
/// </summary>
/// <param name="Ids">需要更新的ids</param>
/// <param name="IsFree">是否限免</param>
public record UpdateIsFreeViewModel(IReadOnlyList<Guid> Ids, FreeEnum IsFree);

/// <summary>
/// 更新发布状态请求
/// </summary>
/// <param name="Ids">需要更新的ids</param>
/// <param name="State">发布状态</param>
public record UpdatePublishStateViewModel(IReadOnlyList<Guid> Ids, StateEnum State);

/// <summary>
/// 更新试卷结构
/// </summary>
/// <param name="Id"></param>
/// <param name="StructJson"></param>
public record UpdatePaperStructViewModel(Guid Id, PaperStructViewModel StructJson);