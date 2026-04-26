namespace HaoKao.ArticleService.Domain.Commands.Article;
/// <summary>
/// 删除文章命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteArticleCommand(
    Guid Id
) : Command("删除文章");