using HaoKao.QuestionCategoryService.Domain.Entities;
using HaoKao.QuestionCategoryService.Domain.Repositories;
using System.Linq;

namespace HaoKao.QuestionCategoryService.Infrastructure.Repositories;

public class QuestionCategoryRepository : Repository<QuestionCategory>, IQuestionCategoryRepository
{
    public IQueryable<QuestionCategory> Query => Queryable.AsNoTracking();
}