using HaoKao.QuestionService.Domain.QuestionModule;

namespace HaoKao.QuestionService.Application.QuestionModule.ViewModels;

[AutoMapTo(typeof(CreateQuestionCommand))]
public class CreateQuestionViewModel : IDto
{
    /// <summary>
    /// 科目Id
    /// </summary>
    [DisplayName("科目Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 科目名称
    /// </summary>
    [DisplayName("科目名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string SubjectName { get; set; }

    /// <summary>
    /// 章节Id
    /// </summary>
    [DisplayName("章节Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid ChapterId { get; set; }

    /// <summary>
    /// 章节名称
    /// </summary>
    [DisplayName("章节名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(100, ErrorMessage = "{0}长度不能大于{1}")]
    public string ChapterName { get; set; }

    /// <summary>
    /// 小节Id
    /// </summary>
    [DisplayName("小节Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid SectionId { get; set; }

    /// <summary>
    /// 小节名称
    /// </summary>
    [DisplayName("小节名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(100, ErrorMessage = "{0}长度不能大于{1}")]
    public string SectionName { get; set; }

    /// <summary>
    /// 知识点Id
    /// </summary>
    [DisplayName("知识点Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid KnowledgePointId { get; set; }

    /// <summary>
    /// 知识点名称
    /// </summary>
    [DisplayName("知识点名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(100, ErrorMessage = "{0}长度不能大于{1}")]
    public string KnowledgePointName { get; set; }

    /// <summary>
    /// 试题分类Id
    /// </summary>
    [DisplayName("试题分类Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid QuestionCategoryId { get; set; }

    /// <summary>
    /// 试题分类名称
    /// </summary>
    [DisplayName("试题分类名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string QuestionCategoryName { get; set; }

    /// <summary>
    /// 试题类型Id
    /// </summary>
    [DisplayName("试题类型Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid QuestionTypeId { get; set; }

    /// <summary>
    /// 试题类型名称
    /// </summary>
    [DisplayName("试题类型名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string QuestionTypeName { get; set; }

    /// <summary>
    /// 试题内容 (题干)
    /// </summary>
    [DisplayName("试题内容 (题干)")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string QuestionText { get; set; }

    /// <summary>
    /// 试题标题 (管理端使用)
    /// </summary>
    [DisplayName("试题标题 (管理端使用)")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string QuestionTitle { get; set; }

    /// <summary>
    /// 文字解析
    /// </summary>
    [DisplayName("文字解析")]
    public string TextAnalysis { get; set; }

    /// <summary>
    /// 音视频解析
    /// </summary>
    [DisplayName("音视频解析")]
    [MaxLength(1000, ErrorMessage = "{0}长度不能大于{1}")]
    public string MediaAnalysis { get; set; }

    /// <summary>
    /// 能力维度Id
    /// </summary>
    [DisplayName("能力维度Id")]
    public List<Guid> AbilityIds { get; set; }

    /// <summary>
    /// 是否免费
    /// </summary>
    [DisplayName("是否免费")]
    public FreeState? FreeState { get; set; } = Domain.QuestionModule.FreeState.No;

    /// <summary>
    /// 启用状态
    /// </summary>
    [DisplayName("启用状态")]
    [Required(ErrorMessage = "{0}不能为空")]
    public EnableState EnableState { get; set; } = EnableState.Enable;

    /// <summary>
    /// 科目标签Id
    /// </summary>
    [DisplayName("科目标签Id")]
    public Guid? SubjectTagId { get; set; }

    /// <summary>
    /// 试卷标签Id
    /// </summary>
    [DisplayName("试卷标签Id")]
    public Guid? PaperTagId { get; set; }

    /// <summary>
    /// 父题目Id
    /// </summary>
    [DisplayName("父题目Id")]
    public Guid? ParentId { get; set; }

    /// <summary>
    /// 试题选项
    /// </summary>
    [DisplayName("试题选项")]
    public object QuestionOptions { get; set; }
}

[AutoMapTo(typeof(UpdateQuestionCommand))]
public class UpdateQuestionViewModel : CreateQuestionViewModel
{
    /// <summary>
    /// 试题Id
    /// </summary>
    [DisplayName("试题Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid Id { get; set; }
}