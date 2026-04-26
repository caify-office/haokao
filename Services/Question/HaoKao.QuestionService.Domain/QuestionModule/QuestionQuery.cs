namespace HaoKao.QuestionService.Domain.QuestionModule;

public class QuestionQuery : QueryBase<Question>
{
    /// <summary>
    /// 科目Id
    /// </summary>
    [QueryCacheKey]
    public Guid? SubjectId { get; set; }

    /// <summary>
    /// 章节Id
    /// </summary>
    [QueryCacheKey]
    public Guid? ChapterId { get; set; }

    /// <summary>
    /// 小节Id
    /// </summary>
    [QueryCacheKey]
    public Guid? SectionId { get; set; }

    /// <summary>
    /// 知识点Id
    /// </summary>
    [QueryCacheKey]
    public Guid? KnowledgePointId { get; set; }

    /// <summary>
    /// 试题标题 (管理端使用)
    /// </summary>
    [QueryCacheKey]
    public string QuestionTitle { get; set; }

    /// <summary>
    /// 试题类型
    /// </summary>
    [QueryCacheKey]
    public Guid? QuestionTypeId { get; set; }

    /// <summary>
    /// 试题分类
    /// </summary>
    [QueryCacheKey]
    public Guid? QuestionCategoryId { get; set; }

    /// <summary>
    /// 启用状态
    /// </summary>
    [QueryCacheKey]
    public EnableState? EnableState { get; set; }

    /// <summary>
    /// 免费专区
    /// </summary>
    [QueryCacheKey]
    public FreeState? FreeState { get; set; }

    /// <summary>
    /// 父Id
    /// </summary>
    [QueryCacheKey]
    public Guid? ParentId { get; set; }

    /// <summary>
    /// 能力维度
    /// </summary>
    [QueryCacheKey]
    public Guid? AbilityId { get; set; }

    /// <summary>
    /// 知识点
    /// </summary>
    [QueryCacheKey]
    public string KnowledgePointName { get; set; }

    /// <summary>
    /// 试题Id集合
    /// </summary>
    public IReadOnlyList<Guid> QuestionIds { get; set; } = new List<Guid>(0);

    [QueryCacheKey]
    public string QuestionIdsString => string.Join(",", QuestionIds);

    /// <summary>
    /// 章节Id集合
    /// </summary>
    public IReadOnlyList<Guid> ChapterNodeIds { get; set; } = new List<Guid>(0);

    [QueryCacheKey]
    public string ChapterNodeIdsString => string.Join(",", ChapterNodeIds);

    [QueryCacheKey]
    public bool? HasVideo { get; set; }

    /// <summary>
    /// 科目标签Id
    /// </summary>
    [QueryCacheKey]
    public Guid? SubjectTagId { get; set; }

    /// <summary>
    /// 试卷标签Id
    /// </summary>
    [QueryCacheKey]
    public Guid? PaperTagId { get; set; }

    public override Expression<Func<Question, bool>> GetQueryWhere()
    {
        Expression<Func<Question, bool>> expression = x => true;

        // (通过QuestionIds查询符合条件的试题时，当ParentId为空时，不需要把它当作搜索条件) 解决bug临时处理方案： 试卷无法显示案例分析的题
        if (!QuestionIds.Any())
        {
            expression = expression.And(x => x.ParentId == ParentId);
        }

        if (SubjectId.HasValue)
        {
            expression = expression.And(x => x.SubjectId == SubjectId);
        }
        if (ChapterId.HasValue)
        {
            expression = expression.And(x => x.ChapterId == ChapterId);
        }
        if (SectionId.HasValue)
        {
            expression = expression.And(x => x.SectionId == SectionId);
        }
        if (KnowledgePointId.HasValue)
        {
            expression = expression.And(x => x.KnowledgePointId == KnowledgePointId);
        }

        if (!string.IsNullOrEmpty(QuestionTitle))
        {
            expression = expression.And(x => EF.Functions.Like(x.QuestionTitle, $"%{QuestionTitle}%"));
        }
        if (QuestionTypeId.HasValue)
        {
            expression = expression.And(x => x.QuestionTypeId == QuestionTypeId.Value);
        }
        if (QuestionCategoryId.HasValue)
        {
            expression = expression.And(x => x.QuestionCategoryId == QuestionCategoryId);
        }
        if (EnableState.HasValue)
        {
            expression = expression.And(x => x.EnableState == EnableState);
        }
        if (FreeState.HasValue)
        {
            expression = expression.And(x => x.FreeState == FreeState);
        }
        if (AbilityId.HasValue)
        {
            expression = expression.And(x => EF.Functions.Like(x.AbilityIds, $"%{AbilityId}%"));
        }
        if (!KnowledgePointName.IsNullOrWhiteSpace())
        {
            expression = expression.And(x => EF.Functions.Like(x.KnowledgePointName, $"%{KnowledgePointName}%"));
        }
        if (QuestionIds.Any())
        {
            expression = expression.And(x => QuestionIds.Contains(x.Id));
        }
        if (ChapterNodeIds.Any())
        {
            expression = expression.And(x => ChapterNodeIds.Contains(x.ChapterId));
        }
        if (HasVideo == true)
        {
            expression = expression.And(x => x.MediaAnalysis != null && !string.IsNullOrEmpty(x.MediaAnalysis));
        }
        if (HasVideo == false)
        {
            expression = expression.And(x => x.MediaAnalysis == null || string.IsNullOrEmpty(x.MediaAnalysis));
        }
        if (SubjectTagId.HasValue)
        {
            expression = expression.And(x => x.SubjectTagId == SubjectTagId);
        }
        if (PaperTagId.HasValue)
        {
            expression = expression.And(x => x.PaperTagId == PaperTagId);
        }
        return expression;
    }
}