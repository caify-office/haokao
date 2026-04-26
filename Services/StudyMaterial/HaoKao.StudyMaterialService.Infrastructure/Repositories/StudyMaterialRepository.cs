using HaoKao.StudyMaterialService.Domain.Entities;
using HaoKao.StudyMaterialService.Domain.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace HaoKao.StudyMaterialService.Infrastructure.Repositories;

public class StudyMaterialRepository : Repository<StudyMaterial>, IStudyMaterialRepository
{
    public IQueryable<StudyMaterial> StudyMaterials => Queryable.AsNoTracking();

    public Task<int> ExecuteUpdateAsync(Expression<Func<StudyMaterial, bool>> predicate, Expression<Func<SetPropertyCalls<StudyMaterial>, SetPropertyCalls<StudyMaterial>>> setPropertyCalls)
    {
        return Queryable.Where(predicate).ExecuteUpdateAsync(setPropertyCalls);
    }
}