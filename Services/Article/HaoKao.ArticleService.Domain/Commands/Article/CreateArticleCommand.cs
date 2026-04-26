namespace HaoKao.ArticleService.Domain.Commands.Article;
/// <summary>
/// 创建文章命令
/// </summary>
/// <param name="Title">标题</param>
/// <param name="Column">所属栏目</param>
/// <param name="Category">所属类别</param>
/// <param name="IsTopping">是否置顶</param>
/// <param name="Sort">排序</param>
/// <param name="IsDisplayedOnHomepage">是否首页展示</param>
/// <param name="IsPublish">是否发布</param>
/// <param name="PreviewUrl">预览图</param>
/// <param name="Content">内容</param>
/// <param name="IsExternalURL">是否外部链接</param>
public record CreateArticleCommand(
    string Title,
    Guid Column,
    Guid Category,
    bool IsTopping,
    int Sort,
    bool IsDisplayedOnHomepage,
    bool IsPublish,
    string PreviewUrl,
    string Content,
    bool IsExternalURL
) : Command("创建文章")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {

        validator.RuleFor(x => Title)
            .NotEmpty().WithMessage("标题不能为空")
            .MaximumLength(50).WithMessage("标题长度不能大于50")
            .MinimumLength(2).WithMessage("标题长度不能小于2");
    }
}