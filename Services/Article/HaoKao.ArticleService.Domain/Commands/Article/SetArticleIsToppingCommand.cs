namespace HaoKao.ArticleService.Domain.Commands.Article;
/// <summary>
/// 设置是否置顶
/// </summary>
/// <param name="Id">主键</param>
/// <param name="IsTopping">是否置顶</param>

public record SetArticleIsToppingCommand(
   Guid Id,
   bool IsTopping

) : Command("设置是否置顶")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {

       
    }
}