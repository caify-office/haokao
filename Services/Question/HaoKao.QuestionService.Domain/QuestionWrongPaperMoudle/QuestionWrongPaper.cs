namespace HaoKao.QuestionService.Domain.QuestionWrongPaperMoudle;

/// <summary>
/// 错题组卷实体类
/// </summary>
public class QuestionWrongPaper : AggregateRoot<Guid>,
                                  IIncludeCreateTime,
                                  IIncludeCreatorId<Guid>,
                                  IIncludeMultiTenant<Guid>,
                                  ITenantShardingTable
{
    /// <summary>
    /// 科目Id
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 试卷名称
    /// </summary>
    public string PaperName { get; set; }

    /// <summary>
    /// 试卷下载地址
    /// </summary>
    public Uri DownloadUrl { get; set; }

    /// <summary>
    /// 试题数量
    /// </summary>
    public int QuestionCount { get; set; }

    /// <summary>
    /// 创建者Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}