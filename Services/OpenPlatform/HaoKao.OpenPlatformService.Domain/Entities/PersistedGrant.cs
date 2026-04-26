namespace HaoKao.OpenPlatformService.Domain.Entities;

public class PersistedGrant : AggregateRoot<Guid>
{
    public string Key { get; init; }

    public string Type { get; init; }

    public string SubjectId { get; init; }

    public string SessionId { get; init; }

    public string ClientId { get; init; }

    public string Description { get; init; }

    public DateTime CreationTime { get; init; }

    public DateTime? Expiration { get; init; }

    public DateTime? ConsumedTime { get; set; }

    public string Data { get; init; }
}