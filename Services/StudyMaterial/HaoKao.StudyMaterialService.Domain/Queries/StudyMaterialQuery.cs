using HaoKao.StudyMaterialService.Domain.Entities;

namespace HaoKao.StudyMaterialService.Domain.Queries;

public class StudyMaterialQuery : QueryBase<StudyMaterial>
{
    /// <summary>
    /// 资料名称
    /// </summary>
    [QueryCacheKey]
    public string Name { get; set; }

    /// <summary>
    /// 所属科目
    /// </summary>
    [QueryCacheKey]
    public Guid? SubjectId { get; set; }

    /// <summary>
    /// 年份
    /// </summary>
    [QueryCacheKey]
    public string Year { get; set; }

    public override Expression<Func<StudyMaterial, bool>> GetQueryWhere()
    {
        Expression<Func<StudyMaterial, bool>> expression = x => true;

        if (!string.IsNullOrEmpty(Name))
        {
            expression = expression.And(x => EF.Functions.Like(x.Name, $"%{Name}%"));
        }
        if (SubjectId.HasValue)
        {
            expression = expression.And(x => EF.Functions.Like(x.Subjects, $"%{SubjectId}%"));
        }
        if (!string.IsNullOrEmpty(Year) && int.TryParse(Year, out var y))
        {
            expression = expression.And(x => x.Year == y);
        }

        return expression;
    }
}