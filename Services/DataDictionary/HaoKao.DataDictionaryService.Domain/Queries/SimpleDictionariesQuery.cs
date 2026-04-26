using HaoKao.DataDictionaryService.Domain.Entities;

namespace HaoKao.DataDictionaryService.Domain.Queries;

public class SimpleDictionariesQuery : QueryBase<Dictionaries>
{
    public override Expression<Func<Dictionaries, bool>> GetQueryWhere()
    {
        Expression<Func<Dictionaries, bool>> expression = x => true;
        return expression;
    }
}