using HaoKao.LiveBroadcastService.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HaoKao.LiveBroadcastService.Domain.Commands.LiveProductPackage;

/// <summary>
/// 批量更新直播产品包命令
/// </summary>
/// <param name="models"></param>
public record UpdateLiveProductPackageCommand(
    List<UpdateLiveProductPackageModel> models
) : Command("批量更新直播产品包")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator) { }
}

public class UpdateLiveProductPackageModel
{
    public Guid Id { get; set; }

    /// <summary>
    /// 所属视频直播Id
    /// </summary>
    [DisplayName("所属视频直播Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid LiveVideoId { get; set; }

    /// <summary>
    /// 产品包Id
    /// </summary>
    [DisplayName("产品包Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid ProductPackageId { get; set; }

    /// <summary>
    /// 产品包名称
    /// </summary>
    [DisplayName("产品包名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string ProductPackageName { get; set; }

    /// <summary>
    /// 产品包类别
    /// </summary>
    [DisplayName("产品包类别")]
    [Required(ErrorMessage = "{0}不能为空")]
    public ProductType ProductType { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [DisplayName("排序")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int Sort { get; set; }
}