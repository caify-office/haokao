namespace HaoKao.QuestionService.Domain.QuestionModule;

/// <summary>
/// 创建试题实体命令
/// </summary>
/// <param name="SubjectId">科目Id</param>
/// <param name="SubjectName">科目名称</param>
/// <param name="ChapterId">章节Id</param>
/// <param name="ChapterName">章节名称</param>
/// <param name="SectionId">小节Id</param>
/// <param name="SectionName">小节名称</param>
/// <param name="KnowledgePointId">知识点Id</param>
/// <param name="KnowledgePointName">知识点名称</param>
/// <param name="QuestionCategoryId">试题分类Id</param>
/// <param name="QuestionCategoryName">试题分类名称</param>
/// <param name="QuestionTypeId">试题类型Id</param>
/// <param name="QuestionTypeName">试题类型名称</param>
/// <param name="QuestionText">试题内容 (题干)</param>
/// <param name="QuestionTitle">试题标题 (管理端使用)</param>
/// <param name="TextAnalysis">文字解析</param>
/// <param name="MediaAnalysis">音视频解析</param>
/// <param name="AbilityIds">能力维度Id</param>
/// <param name="FreeState">免费分区</param>
/// <param name="EnableState">启用状态</param>
/// <param name="SubjectTagId">科目标签Id</param>
/// <param name="PaperTagId">试卷标签Id</param>
/// <param name="ParentId">父题目Id</param>
/// <param name="QuestionOptions">试题选项</param>
public record CreateQuestionCommand(
    Guid SubjectId,
    string SubjectName,
    Guid ChapterId,
    string ChapterName,
    Guid SectionId,
    string SectionName,
    Guid KnowledgePointId,
    string KnowledgePointName,
    Guid QuestionCategoryId,
    string QuestionCategoryName,
    Guid QuestionTypeId,
    string QuestionTypeName,
    string QuestionText,
    string QuestionTitle,
    string TextAnalysis,
    string MediaAnalysis,
    List<Guid> AbilityIds,
    FreeState? FreeState,
    EnableState EnableState,
    Guid? SubjectTagId,
    Guid? PaperTagId,
    Guid? ParentId,
    object QuestionOptions
) : Command("创建试题实体")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => SubjectName)
                 .NotEmpty().WithMessage("科目名称不能为空")
                 .MaximumLength(50).WithMessage("科目名称长度不能大于50");

        validator.RuleFor(x => ChapterName)
                 .NotEmpty().WithMessage("章节名称不能为空")
                 .MaximumLength(50).WithMessage("章节名称长度不能大于100");

        validator.RuleFor(x => SectionName)
                 .NotEmpty().WithMessage("小节名称不能为空")
                 .MaximumLength(50).WithMessage("小节名称长度不能大于100");

        validator.RuleFor(x => KnowledgePointName)
                 .NotEmpty().WithMessage("知识点名称不能为空")
                 .MaximumLength(50).WithMessage("知识点名称长度不能大于50");

        validator.RuleFor(x => QuestionCategoryName)
                 .NotEmpty().WithMessage("试题分类名称不能为空")
                 .MaximumLength(50).WithMessage("试题分类名称长度不能大于50");

        validator.RuleFor(x => QuestionTypeName)
                 .NotEmpty().WithMessage("试题类型名称不能为空")
                 .MaximumLength(50).WithMessage("试题类型名称长度不能大于50");

        validator.RuleFor(x => QuestionText)
                 .NotEmpty().WithMessage("试题内容 (题干)不能为空")
                 .Must(HasImage).WithMessage("存在非法图片，保存失败");

        return;

        static bool HasImage(string field)
        {
            return field == null || !field.Contains("data:image/png;base64");
        }
    }
}

/// <summary>
/// 更新试题实体命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="SubjectId">科目Id</param>
/// <param name="SubjectName">科目名称</param>
/// <param name="ChapterId">章节Id</param>
/// <param name="ChapterName">章节名称</param>
/// <param name="SectionId">小节Id</param>
/// <param name="SectionName">小节名称</param>
/// <param name="KnowledgePointId">知识点Id</param>
/// <param name="KnowledgePointName">知识点名称</param>
/// <param name="QuestionCategoryId">试题分类Id</param>
/// <param name="QuestionCategoryName">试题分类名称</param>
/// <param name="QuestionTypeId">试题类型Id</param>
/// <param name="QuestionTypeName">试题类型名称</param>
/// <param name="QuestionText">试题内容 (题干)</param>
/// <param name="QuestionTitle">试题标题 (管理端使用)</param>
/// <param name="TextAnalysis">文字解析</param>
/// <param name="MediaAnalysis">音视频解析</param>
/// <param name="AbilityIds">能力维度Id</param>
/// <param name="FreeState">免费分区</param>
/// <param name="EnableState">启用状态</param>
/// <param name="SubjectTagId">科目标签Id</param>
/// <param name="PaperTagId">试卷标签Id</param>
/// <param name="ParentId">父题目Id</param>
/// <param name="QuestionOptions">试题选项</param>
public record UpdateQuestionCommand(
    Guid Id,
    Guid SubjectId,
    string SubjectName,
    Guid ChapterId,
    string ChapterName,
    Guid SectionId,
    string SectionName,
    Guid KnowledgePointId,
    string KnowledgePointName,
    Guid QuestionCategoryId,
    string QuestionCategoryName,
    Guid QuestionTypeId,
    string QuestionTypeName,
    string QuestionText,
    string QuestionTitle,
    string TextAnalysis,
    string MediaAnalysis,
    List<Guid> AbilityIds,
    FreeState? FreeState,
    EnableState EnableState,
    Guid? SubjectTagId,
    Guid? PaperTagId,
    Guid? ParentId,
    object QuestionOptions
) : Command("更新试题实体")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => SubjectName)
                 .NotEmpty().WithMessage("科目名称不能为空")
                 .MaximumLength(50).WithMessage("科目名称长度不能大于50");

        validator.RuleFor(x => ChapterName)
                 .NotEmpty().WithMessage("章节名称不能为空")
                 .MaximumLength(50).WithMessage("章节名称长度不能大于100");

        validator.RuleFor(x => SectionName)
                 .NotEmpty().WithMessage("小节名称不能为空")
                 .MaximumLength(50).WithMessage("小节名称长度不能大于100");

        validator.RuleFor(x => KnowledgePointName)
                 .NotEmpty().WithMessage("知识点名称不能为空")
                 .MaximumLength(50).WithMessage("知识点名称长度不能大于50");

        validator.RuleFor(x => QuestionCategoryName)
                 .NotEmpty().WithMessage("试题分类名称不能为空")
                 .MaximumLength(50).WithMessage("试题分类名称长度不能大于50");

        validator.RuleFor(x => QuestionText)
                 .NotEmpty().WithMessage("试题内容 (题干)不能为空")
                 .Must(HasImage).WithMessage("存在非法图片，保存失败");

        return;

        static bool HasImage(string field)
        {
            return field == null || !field.Contains("data:image/png;base64");
        }
    }
}

/// <summary>
/// 批量删除试题实体
/// </summary>
/// <param name="Ids"></param>
public record DeleteQuestionCommand(IEnumerable<Guid> Ids) : Command("批量删除试题实体");

/// <summary>
/// 修改试题排序
/// </summary>
/// <param name="Id">试题Id</param>
/// <param name="Sort">排序</param>
public record UpdateSortCommand(Guid Id, int Sort) : Command("修改试题排序");

/// <summary>
/// 批量修改免费专区
/// </summary>
/// <param name="Ids">试题Ids</param>
/// <param name="FreeState">是否免费</param>
public record UpdateFreeStateCommand(IEnumerable<Guid> Ids, FreeState FreeState) : Command("批量修改免费专区");

/// <summary>
/// 批量修改启用状态
/// </summary>
/// <param name="Ids">试题Ids</param>
/// <param name="EnableState">是否启用</param>
public record UpdateEnableStateCommand(IEnumerable<Guid> Ids, EnableState EnableState) : Command("批量修改启用状态");