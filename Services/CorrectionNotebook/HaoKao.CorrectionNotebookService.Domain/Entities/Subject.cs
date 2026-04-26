using System.ComponentModel.DataAnnotations.Schema;

namespace HaoKao.CorrectionNotebookService.Domain.Entities;

/// <summary>
/// 科目实体类
/// </summary>
public sealed class Subject : AggregateRoot<Guid>,
                              IIncludeCreatorId<Guid>,
                              IIncludeCreateTime
{
    /// <summary>
    /// 考试级别Id
    /// </summary>
    public Guid ExamLevelId { get; init; }

    /// <summary>
    /// 科目名称
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 科目图标
    /// </summary>
    public string Icon { get; init; }

    /// <summary>
    /// 是否内置数据
    /// </summary>
    public bool IsBuiltIn { get; init; }

    /// <summary>
    /// 创建人Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 题目数量
    /// </summary>
    [NotMapped]
    public int QuestionCount { get; init; }

    /// <summary>
    /// 排序
    /// </summary>
    [NotMapped]
    public SubjectSort Sort { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public List<SubjectSort> Sorts { get; init; } = [];

    /// <summary>
    /// 试题集合
    /// </summary>
    public List<Question> Questions { get; init; } = [];

    internal static Subject Create(Guid examLevelId, string name, Guid userId, int sort)
    {
        var subject = new Subject
        {
            ExamLevelId = examLevelId,
            Name = name,
            IsBuiltIn = false,
            CreatorId = userId,
            CreateTime = DateTime.Now,
            Icon = "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/%E9%80%9A%E7%94%A8%402x.png",
        };

        subject.Sorts.Add(SubjectSort.Create(userId, subject.Id, sort));

        return subject;
    }

    public void AddQuestion(Uri imageUrl, Guid userId, IReadOnlyList<Tag> tags)
    {
        var question = Question.Create(ExamLevelId, Id, imageUrl, userId, tags);
        Questions.Add(question);
    }
}