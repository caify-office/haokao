namespace HaoKao.ArticleService.Domain.Commands.ArticleBrowseRecord;

/// <summary>
/// 创建文章浏览记录命令
/// </summary>
/// <param name="ClientUniqueId"></param>
/// <param name="ArticleId">文章Id</param>
public record CreateArticleBrowseRecordCommand(
    Guid ClientUniqueId,
    Guid ArticleId
) : Command("创建文章浏览记录")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator) { }
}