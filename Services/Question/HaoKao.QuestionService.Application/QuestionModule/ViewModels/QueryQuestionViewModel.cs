using HaoKao.QuestionService.Domain.QuestionModule;

namespace HaoKao.QuestionService.Application.QuestionModule.ViewModels;

[AutoMapFrom(typeof(QuestionQuery))]
[AutoMapTo(typeof(QuestionQuery))]
public class QueryQuestionViewModel : QueryDtoBase<BrowseQuestionViewModel>
{
    /// <summary>
    /// 科目Id
    /// </summary>
    [JsonIgnore]
    public Guid? SubjectId { get; set; }

    /// <summary>
    /// 章节Id
    /// </summary>
    [JsonIgnore]
    public Guid? ChapterId { get; set; }

    /// <summary>
    /// 小节Id
    /// </summary>
    [JsonIgnore]
    public Guid? SectionId { get; set; }

    /// <summary>
    /// 知识点Id
    /// </summary>
    [JsonIgnore]
    public Guid? KnowledgePointId { get; set; }

    /// <summary>
    /// 试题标题 (管理端使用)
    /// </summary>
    [JsonIgnore]
    public string QuestionTitle { get; set; }

    /// <summary>
    /// 试题类型Id
    /// </summary>
    [JsonIgnore]
    public Guid? QuestionTypeId { get; set; }

    /// <summary>
    /// 试题分类
    /// </summary>
    [JsonIgnore]
    public Guid? QuestionCategoryId { get; set; }

    /// <summary>
    /// 启用状态
    /// </summary>
    [JsonIgnore]
    public EnableState? EnableState { get; set; }

    /// <summary>
    /// 免费专区
    /// </summary>
    [JsonIgnore]
    public FreeState? FreeState { get; set; }

    /// <summary>
    /// 科目标签Id
    /// </summary>
    [JsonIgnore]
    public Guid? SubjectTagId { get; set; }

    /// <summary>
    /// 试卷标签Id
    /// </summary>
    [JsonIgnore]
    public Guid? PaperTagId { get; set; }

    /// <summary>
    /// 父Id
    /// </summary>
    [JsonIgnore]
    public Guid? ParentId { get; set; }

    /// <summary>
    /// 是否有解析视频
    /// </summary>
    [JsonIgnore]
    public bool? HasVideo { get; set; }
}

[AutoMapFrom(typeof(Question))]
[AutoMapTo(typeof(Question))]
public class BrowseQuestionViewModel : IDto
{
    /// <summary>
    /// 试题Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 科目Id
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 科目名称
    /// </summary>
    public string SubjectName { get; set; }

    /// <summary>
    /// 章节Id
    /// </summary>
    public Guid ChapterId { get; set; }

    /// <summary>
    /// 章节名称
    /// </summary>
    public string ChapterName { get; set; }

    /// <summary>
    /// 小节Id
    /// </summary>
    public Guid SectionId { get; set; }

    /// <summary>
    /// 小节名称
    /// </summary>
    public string SectionName { get; set; }

    /// <summary>
    /// 知识点Id
    /// </summary>
    public Guid KnowledgePointId { get; set; }

    /// <summary>
    /// 知识点名称
    /// </summary>
    public string KnowledgePointName { get; set; }

    /// <summary>
    /// 试题分类Id
    /// </summary>
    public Guid QuestionCategoryId { get; set; }

    /// <summary>
    /// 试题分类名称
    /// </summary>
    public string QuestionCategoryName { get; set; }

    /// <summary>
    /// 试题类型Id
    /// </summary>
    public Guid QuestionTypeId { get; set; }

    /// <summary>
    /// 试题类型名称
    /// </summary>
    public string QuestionTypeName { get; set; }

    /// <summary>
    /// 试题内容 (题干)
    /// </summary>
    public string QuestionText { get; set; }

    /// <summary>
    /// 试题标题 (管理端使用)
    /// </summary>
    public string QuestionTitle { get; set; }

    /// <summary>
    /// 文字解析
    /// </summary>
    public string TextAnalysis { get; set; }

    /// <summary>
    /// 试题音视频解析
    /// </summary>
    public string MediaAnalysis { get; set; }

    /// <summary>
    /// 是否免费
    /// </summary>
    public FreeState? FreeState { get; set; }

    /// <summary>
    /// 启用状态
    /// </summary>
    public EnableState EnableState { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 试题数量
    /// </summary>
    public int QuestionCount { get; set; }

    /// <summary>
    /// 能力维度Id
    /// </summary>
    [JsonIgnore]
    public string AbilityIds { get; set; }

    [JsonPropertyName("abilityIds")]
    public List<string> AbilityList => AbilityIds?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

    /// <summary>
    /// 试题选项
    /// </summary>
    [JsonIgnore]
    public string QuestionOptions { get; set; }

    [JsonPropertyName("questionOptions")]
    public dynamic QuestionOptionsObj => JsonSerializer.Deserialize<dynamic>(QuestionOptions);

    /// <summary>
    /// 科目标签Id
    /// </summary>
    public Guid? SubjectTagId { get; set; }

    /// <summary>
    /// 试卷标签Id
    /// </summary>
    public Guid? PaperTagId { get; set; }

    /// <summary>
    /// 父题目Id
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 创建者Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime UpdateTime { get; set; }
}