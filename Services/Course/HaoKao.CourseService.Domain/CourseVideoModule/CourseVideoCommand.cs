namespace HaoKao.CourseService.Domain.CourseVideoModule;

/// <summary>
/// 创建课程视频命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="CourseChapterId">关联的课程id</param>
/// <param name="KnowledgePointId">知识点Id</param>
/// <param name="Suffix">后缀</param>
/// <param name="Duration">时长</param>
/// <param name="VideoName">视频名称</param>
/// <param name="SourceName">视频源名称</param>
/// <param name="VideoUrl">播放url-冗余</param>
/// <param name="VideoId">视频id</param>
/// <param name="DisplayName"></param>
/// <param name="CateId">视频分类id</param>
/// <param name="CateName">视频分类名称</param>
/// <param name="Tags">视频标签</param>
public record SaveCourseVideoCommand(
    Guid CourseChapterId,
    Guid KnowledgePointId,
    string Suffix,
    decimal Duration,
    string VideoName,
    string SourceName,
    string VideoUrl,
    string VideoId,
    string DisplayName,
    long?  CateId,
    string CateName,
    string Tags
) : Command("创建课程视频")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => VideoName).NotEmpty().WithMessage("视频名称不能为空");
        validator.RuleFor(x => VideoId).NotEmpty().WithMessage("视频id不能为空");
    }
}

/// <summary>
/// 批量增加
/// </summary>
/// <param name="Models">视频对象集合</param>
public record CreateCourseVideoBatchCommand(List<CourseVideo> Models) : Command("批量增加");

/// <summary>
/// 更新课程视频命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="Suffix">后缀</param>
/// <param name="Duration">时长</param>
/// <param name="VideoName">视频名称</param>
/// <param name="SourceName">视频源名称</param>
/// <param name="VideoUrl">播放url-冗余</param>
/// <param name="VideoId">视频id</param>
/// <param name="DisplayName">显示名称</param>
/// <param name="CateId">视频分类id</param>
/// <param name="CateName">视频分类名称</param>
/// <param name="Tags">视频标签</param>
public record UpdateCourseVideoCommand(
    Guid Id,
    string Suffix,
    decimal Duration,
    string VideoName,
    string SourceName,
    string VideoUrl,
    string VideoId,
    string DisplayName,
    long? CateId,
    string CateName,
    string Tags
) : Command("更新课程视频")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => VideoName).NotEmpty().WithMessage("视频名称不能为空");
        validator.RuleFor(x => VideoId).NotEmpty().WithMessage("视频id不能为空");
    }
}

/// <summary>
/// 试听修改
/// </summary>
/// <param name="Ids">视频ids</param>
/// <param name="State">修改试听状态</param>
public record UpdateIsTryCommand(IEnumerable<Guid> Ids, bool State) : Command("试听修改");

/// <summary>
/// 修改排序
/// </summary>
/// <param name="Id"></param>
/// <param name="Sort"></param>
public record UpdateSortCommand(Guid Id, int Sort) : Command("修改排序");

/// <summary>
/// 知识点修改
/// </summary>
/// <param name="Id">视频id</param>
/// <param name="KnowledgePointIds">知识点ids</param>
public record UpdateKnowledgePointCommand(Guid Id, string KnowledgePointIds) : Command("知识点修改");

/// <summary>
/// 批量修改前缀
/// </summary>
/// <param name="Ids"></param>
/// <param name="Name"></param>
public record BatchUpdateNameCommand(IEnumerable<Guid> Ids, string Name) : Command("批量修改前缀");

/// <summary>
/// 删除课程视频命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteCourseVideoCommand(Guid Id) : Command("删除课程视频");