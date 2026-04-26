namespace HaoKao.NotificationMessageService.Domain.Models;

public class TenantSignSetting : AggregateRoot<Guid>, IIncludeMultiTenant<Guid>
{
    public Guid TenantId { get; set; }

    /// <summary>
    /// 签名名称
    /// </summary>
    public string Sign { get; set; }
}