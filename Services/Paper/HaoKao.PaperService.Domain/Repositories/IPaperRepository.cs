using HaoKao.PaperService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;

namespace HaoKao.PaperService.Domain.Repositories;

public interface IPaperRepository : IRepository<Paper>
{
    IQueryable<Paper> Query { get; }

    /// <summary>
    /// 按科目和类别获取试卷列表
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    Task<List<Paper>> GetPaperList(Guid subjectId, Guid categoryId);

    /// <summary>
    /// 按科目获取试卷数量和试题数量
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    Task<string> GetPaperCountAndPaperQuestionCount(Guid subjectId);

    Task<int> ExecuteUpdateAsync(Expression<Func<Paper, bool>> predicate, Expression<Func<SetPropertyCalls<Paper>, SetPropertyCalls<Paper>>> setPropertyCalls);
}