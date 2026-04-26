namespace HaoKao.QuestionService.Application.QuestionWrongModule.ViewModels;

public record QueryTodayTaskViewModel : IDto
{
    /// <summary>
    /// 科目Id
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 每日任务题量
    /// </summary>
    [DisplayName("每日任务题量")]
    [Range(1, 99, ErrorMessage = "{0}可设置1~99")]
    public int Count { get; set; }
}

public record QueryChapterViewModel(Guid SubjectId, bool IsActive) : IDto;

public record QueryChapterQuestionViewModel(Guid ChapterId, bool IsActive) : IDto;