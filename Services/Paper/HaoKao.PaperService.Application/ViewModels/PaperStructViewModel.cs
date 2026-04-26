namespace HaoKao.PaperService.Application.ViewModels;

public record PaperStructViewModel
{
    [JsonPropertyName("templeteId")]
    public string TempleteId { get; init; }

    [JsonPropertyName("templeteName")]
    public string TempleteName { get; init; }

    [JsonPropertyName("templeteStructDatas")]
    public IReadOnlyList<TempleteStructData> TempleteStructDatas { get; init; }

    [JsonPropertyName("templeteCount")]
    public int TempleteCount { get; init; }

    [JsonPropertyName("templeteScore")]
    public decimal TempleteScore { get; init; }

    [JsonPropertyName("paperCount")]
    public int PaperCount { get; init; }

    [JsonPropertyName("paperScore")]
    public decimal PaperScore { get; init; }
}

public record TempleteStructData
{
    [JsonPropertyName("basicInfo")]
    public BasicInfo BasicInfo { get; init; }

    [JsonPropertyName("settingInfo")]
    public SettingInfo SettingInfo { get; init; }

    [JsonPropertyName("scoringRules")]
    public ScoringRule ScoringRules { get; init; }

    [JsonPropertyName("questions")]
    public IReadOnlyList<QuestionInfo> Questions { get; init; }

    [JsonPropertyName("code")]
    public int Code { get; init; }
}

public record BasicInfo
{
    [JsonPropertyName("typeId")]
    public string TypeId { get; init; }

    [JsonPropertyName("typeName")]
    public string TypeName { get; init; }

    [JsonPropertyName("typeDescription")]
    public string TypeDescription { get; init; }

    [JsonPropertyName("questionCount")]
    public int QuestionCount { get; init; }

    [JsonPropertyName("questionScore")]
    public decimal QuestionScore { get; init; }

    [JsonPropertyName("totalScore")]
    public decimal TotalScore { get; init; }
}

public record SettingInfo
{
    [JsonPropertyName("questionCount")]
    public int QuestionCount { get; init; }

    [JsonPropertyName("totalScore")]
    public decimal TotalScore { get; init; }
}

public record ScoringRule
{
    [JsonPropertyName("Correct")]
    public decimal Correct { get; init; }

    [JsonPropertyName("Missing")]
    public decimal Missing { get; init; }

    [JsonPropertyName("Wrong")]
    public decimal Wrong { get; init; }

    [JsonPropertyName("Lack")]
    public decimal Lack { get; init; }
}

public record QuestionInfo
{
    [JsonPropertyName("id")]
    public string Id { get; init; }

    [JsonPropertyName("questionTypeId")]
    public Guid QuestionTypeId { get; init; }

    [JsonPropertyName("scoringRules")]
    public ScoringRule ScoringRules { get; init; }

    [JsonPropertyName("questions")]
    public IReadOnlyList<QuestionInfo> Questions { get; init; }
}