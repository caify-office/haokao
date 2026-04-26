using System.Text.RegularExpressions;
using HaoKao.CorrectionNotebookService.Domain.Enums;
using HaoKao.CorrectionNotebookService.Domain.Services;

namespace HaoKao.CorrectionNotebookService.Domain.Entities;

/// <summary>
/// 题目实体类
/// </summary>
public sealed partial class Question : AggregateRoot<Guid>,
                                       IIncludeCreatorId<Guid>,
                                       IIncludeCreateTime,
                                       IIncludeUpdateTime
{
    /// <summary>
    /// 题目所属考试级别Id
    /// </summary>
    public Guid ExamLevelId { get; init; }

    /// <summary>
    /// 题目所属科目Id
    /// </summary>
    public Guid SubjectId { get; init; }

    /// <summary>
    /// 题目图片Url
    /// </summary>
    public Uri ImageUrl { get; init; }

    /// <summary>
    /// 题目内容(文本)
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 题目答案
    /// </summary>
    public string Answer { get; set; }

    /// <summary>
    /// 题目解析
    /// </summary>
    public string Analysis { get; set; }

    /// <summary>
    /// 生成次数
    /// </summary>
    public int GenerationTimes { get; set; }

    /// <summary>
    /// 是否可生成答案和解析
    /// </summary>
    public bool Generatable { get; set; }

    /// <summary>
    /// 掌握程度
    /// </summary>
    public MasteryDegree MasteryDegree { get; set; }

    /// <summary>
    /// 创建人Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// 题目关联的标签
    /// </summary>
    public List<QuestionTag> Tags { get; init; } = [];

    /// <summary>
    /// 生成的答案和解析记录
    /// </summary>
    public List<GenerationLog> GenerationLogs { get; init; } = [];

    internal static Question Create(Guid examLevelId, Guid subjectId, Uri imageUrl, Guid creatorId, IReadOnlyList<Tag> tags)
    {
        var id = Guid.NewGuid();
        return new Question
        {
            Id = id,
            ExamLevelId = examLevelId,
            SubjectId = subjectId,
            ImageUrl = imageUrl,
            Content = "",
            Answer = "",
            Analysis = "",
            Generatable = true,
            MasteryDegree = MasteryDegree.NotMastered,
            CreatorId = creatorId,
            CreateTime = DateTime.Now,
            Tags = tags.Select(tag => new QuestionTag
            {
                TagId = tag.Id,
                QuestionId = id
            }).ToList(),
        };
    }

    public async Task GenerateAnswerAndAnalysis(ILargeLanguageModel llm, string data)
    {
        if (!Generatable) return;

        var result = await llm.CompletionAsync(data);

        SaveAnswerAndAnalysis(llm, result);

        GenerationTimes++;
    }

    public async IAsyncEnumerable<string> GenerateAnswerAndAnalysisStream(ILargeLanguageModel llm, string data)
    {
        if (!Generatable) yield break;

        var list = new List<string>();
        var stream = llm.CompletionStreamAsync(data);
        await foreach (var chunk in stream)
        {
            list.Add(chunk);
            yield return chunk;
        }

        var result = llm.ReadStream(list);
        SaveAnswerAndAnalysis(llm, result);

        GenerationTimes++;
    }

    private void SaveAnswerAndAnalysis(ILargeLanguageModel llm, string result)
    {
        // 使用正则匹配答案和解析
        /*
答案: C、A、D、C、D
解析: 
81. 供给弹性系数计算公式为(ΔQ/Q)/(ΔP/P)，代入数据得0.25，选C。
82. 供给弹性小于1，说明供给量变动小于价格变动，属于缺乏弹性，选A。
83. 需求弹性系数同理计算，结果接近0，说明需求量变动很小，需求对价格不敏感，选D。
84. 需求弹性小表示价格变动对需求量影响不大，选C。
85. 生活必需品需求弹性通常较小，因为居民生活需求程度高，选D。
        */
        var match = MyRegex().Match(result);
        if (match.Success)
        {
            Answer = match.Groups["answer"].Value;
            Analysis = match.Groups["analysis"].Value;
        }

        GenerationLogs.Add(new GenerationLog
        {
            QuestionId = Id,
            Answer = Answer,
            Analysis = Analysis,
            CreateTime = DateTime.Now,
            CreatorId = llm.Id,
            CreatorName = llm.Name
        });
    }

    [GeneratedRegex(@"答案[:：]\s*(?<answer>.+?)\s*解析[:：]\s*(?<analysis>[\s\S]+)")]
    private static partial Regex MyRegex();
}