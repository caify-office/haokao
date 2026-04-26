using HaoKao.StudyMaterialService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;

namespace HaoKao.StudyMaterialService.Domain.Repositories;

public interface IStudyMaterialRepository : IRepository<StudyMaterial>
{
    IQueryable<StudyMaterial> StudyMaterials { get; }
    Task<int> ExecuteUpdateAsync(Expression<Func<StudyMaterial, bool>> predicate, Expression<Func<SetPropertyCalls<StudyMaterial>, SetPropertyCalls<StudyMaterial>>> setPropertyCalls);
}