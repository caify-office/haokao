using HaoKao.QuestionService.Domain.QuestionModule;

namespace HaoKao.QuestionService.Application.QuestionModule.ViewModels;

[AutoMapFrom(typeof(Question))]
public class QueryQuestionListAppViewModel : IDto
{
    public Guid Id { get; init; }

    public Guid? ParentId { get; init; }

    public Guid QuestionTypeId { get; init; }

    public Guid QuestionCategoryId { get; init; }

    public Guid ChapterId { get; set; }

    public Guid SectionId { get; set; }

    public Guid KnowledgePointId { get; set; }

    public FreeState? FreeState { get; set; }

    [JsonIgnore]
    public int Sort { get; set; }

    [JsonIgnore]
    public DateTime CreateTime { get; set; }
}

[AutoMapFrom(typeof(Question))]
public class BrowseQuestionAppViewModel : BrowseQuestionViewModel
{
    /// <summary>
    /// 是否已收藏
    /// </summary>
    public bool IsCollected { get; set; }

    /// <summary>
    /// 收藏时间
    /// </summary>
    public DateTime? CollectionTime { get; set; }
}

public record QuerySpecialPromotionQuestionViewModel : IDto
{
    /// <summary>
    /// 科目Id
    /// </summary>
    public Guid SubjectId { get; init; }

    /// <summary>
    /// 能力维度
    /// </summary>
    public string Ability { get; init; }

    /// <summary>
    /// 需要抽取的题目数量
    /// </summary>
    public int Count { get; init; }

    /// <summary>
    /// 是否试用
    /// </summary>
    public bool Trial { get; init; }
}