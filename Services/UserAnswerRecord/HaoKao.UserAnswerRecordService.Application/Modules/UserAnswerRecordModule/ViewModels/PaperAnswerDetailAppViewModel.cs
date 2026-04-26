using HaoKao.UserAnswerRecordService.Domain.Entities;

namespace HaoKao.UserAnswerRecordService.Application.Modules.UserAnswerRecordModule.ViewModels;

[AutoMapFrom(typeof(UserAnswerRecord))]
public class PaperAnswerDetailQueryFieldModel : IDto
{
    /// <summary>
    /// 记录Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 试卷Id
    /// </summary>
    [JsonPropertyName("paperId")]
    public Guid RecordIdentifier { get; set; }

    public decimal UserScore { get; set; }

    public decimal PassingScore { get; set; }

    public decimal TotalScore { get; set; }
}

public class PaperAnswerDetailAppViewModel : PaperAnswerDetailQueryFieldModel
{
    public int FinishCount { get; set; }
}