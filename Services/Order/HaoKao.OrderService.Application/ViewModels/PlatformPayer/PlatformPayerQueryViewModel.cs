using HaoKao.OrderService.Domain.Enums;
using HaoKao.OrderService.Domain.Queries;
using System.Text.Json.Serialization;

namespace HaoKao.OrderService.Application.ViewModels.PlatformPayer;

[AutoMapFrom(typeof(PlatformPayerQuery))]
[AutoMapTo(typeof(PlatformPayerQuery))]
public class PlatformPayerQueryViewModel : QueryDtoBase<PlatformPayerQueryListViewModel>
{
    /// <summary>
    /// 支付名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 对应支付处理者Id
    /// </summary>
    public Guid? PayerId { get; set; }

    /// <summary>
    /// 启用/禁用
    /// </summary>
    public bool? UseState { get; set; }

    /// <summary>
    /// 支付归类
    /// </summary>
    public PaymentMethod? PaymentMethod { get; set; }

    /// <summary>
    /// 使用场景
    /// </summary>
    public PlatformPayerScenes? PlatformPayerScenes { get; set; }

    /// <summary>
    /// 苹果端是否显示
    /// </summary>
    public bool? IosIsOpen { get; set; }

    /// <summary>
    /// ID列表
    /// </summary>
    public List<Guid> Ids { get; set; } = [];
}

[AutoMapFrom(typeof(Domain.Entities.PlatformPayer))]
[AutoMapTo(typeof(Domain.Entities.PlatformPayer))]
public class PlatformPayerQueryListViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 支付名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 对应支付处理者Id
    /// </summary>
    public Guid PayerId { get; set; }

    /// <summary>
    /// 对应支付处理者名称
    /// </summary>
    public string PayerName { get; set; }

    /// <summary>
    /// 支付方式归类
    /// </summary>
    public PaymentMethod PaymentMethod { get; set; }

    /// <summary>
    /// Ios是否开启
    /// </summary>
    public bool IosIsOpen { get; set; }

    /// <summary>
    /// 使用场景
    /// </summary>
    public PlatformPayerScenes PlatformPayerScenes { get; set; }

    /// <summary>
    /// 创建的时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// 启用/禁用
    /// </summary>
    public bool UseState { get; set; }

    /// <summary>
    /// 分期扩展字段
    /// </summary>
    [JsonPropertyName("extend_params")]
    public string ExtendParams { get; set; }
}