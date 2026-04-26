namespace HaoKao.StudentService.Domain.Models;

/// <summary>
/// 其它平台唯一标识符
/// </summary>
public record ExternalIdentity
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 其它平台名称
    /// </summary>
    public string Scheme { get; init; }

    /// <summary>
    /// 唯一标识符
    /// </summary>
    public string UniqueIdentifier { get; init; }

    /// <summary>
    /// 注册用户Id
    /// </summary>
    public Guid RegisterUserId { get; init; }
}