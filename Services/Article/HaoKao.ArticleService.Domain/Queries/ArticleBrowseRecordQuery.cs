using HaoKao.ArticleService.Domain.Entities;

namespace HaoKao.ArticleService.Domain.Queries;

public class ArticleBrowseRecordQuery : QueryBase<ArticleBrowseRecord>
{   
    public override Expression<Func<ArticleBrowseRecord, bool>> GetQueryWhere()
    {
        Expression<Func<ArticleBrowseRecord, bool>> expression = x => true;
        return expression;
    }
}
