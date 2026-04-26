using HaoKao.QuestionService.Domain.QuestionModule;

namespace HaoKao.QuestionService.Application.QuestionModule.ViewModels;

/// <summary>
/// 试卷查看试题
/// </summary>
[AutoMapFrom(typeof(Question))]
[AutoMapTo(typeof(Question))]
public class BrowsePaperViewModel : IDto
{
    /// <summary>
    /// 试题Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 父题目Id
    /// </summary>
    public Guid ParentId { get; set; }

    /// <summary>
    /// 题型Id
    /// </summary>
    public Guid QuestionTypeId { get; set; }

    /// <summary>
    /// 试题内容 (题干)
    /// </summary>
    public string QuestionText { get; set; }

    /// <summary>
    /// 试题数量
    /// </summary>
    public int QuestionCount { get; set; }
}

/// <summary>
/// 试卷引入试题查询
/// </summary>
[AutoMapFrom(typeof(QuestionQuery))]
[AutoMapTo(typeof(QuestionQuery))]
public class QueryPaperViewModel : QueryDtoBase<QueryPaperListViewModel>
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
    /// 能力维度
    /// </summary>
    [JsonIgnore]
    public Guid? AbilityId { get; set; }

    /// <summary>
    /// 题干
    /// </summary>
    [JsonIgnore]
    public string QuestionTitle { get; set; }

    /// <summary>
    /// 知识点
    /// </summary>
    [JsonIgnore]
    public string KnowledgePointName { get; set; }

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
}

/// <summary>
/// 试卷引入试题结果
/// </summary>
[AutoMapFrom(typeof(Question))]
[AutoMapTo(typeof(Question))]
public class QueryPaperListViewModel : IDto
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

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
    /// 试题内容 (题干)
    /// </summary>
    public string QuestionText { get; set; }

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
}