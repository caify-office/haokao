using HaoKao.DataDictionaryService.Domain.Entities;

namespace HaoKao.DataDictionaryService.Domain.Queries;

public class DictionariesQuery : QueryBase<Dictionaries>
{
    /// <summary>
    /// 主键
    /// </summary>
    [QueryCacheKey]
    public Guid? Id { get; set; }

    [QueryCacheKey]
    public string Name { get; set; }

    [QueryCacheKey]
    public bool? State { get; set; }

    public override Expression<Func<Dictionaries, bool>> GetQueryWhere()
    {
        Expression<Func<Dictionaries, bool>> expression = dictionaries => dictionaries.Pid == Id;

        if (!Name.IsNullOrWhiteSpace())
        {
            expression = expression.And(dictionaries => dictionaries.Name.Contains(Name));
        }

        if (State != null)
        {
            expression = expression.And(dictionaries => dictionaries.State == State);
        }

        return expression;
    }
}