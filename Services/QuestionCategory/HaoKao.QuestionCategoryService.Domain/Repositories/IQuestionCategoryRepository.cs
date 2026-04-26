using HaoKao.QuestionCategoryService.Domain.Entities;
using System.Linq;

namespace HaoKao.QuestionCategoryService.Domain.Repositories;

public interface IQuestionCategoryRepository : IRepository<QuestionCategory>
{
    public IQueryable<QuestionCategory> Query { get; }
}