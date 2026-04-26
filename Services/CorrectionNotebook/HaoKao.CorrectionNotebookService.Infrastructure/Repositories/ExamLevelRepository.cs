using HaoKao.CorrectionNotebookService.Domain.Entities;
using HaoKao.CorrectionNotebookService.Domain.Queries;
using HaoKao.CorrectionNotebookService.Domain.Repositories;

namespace HaoKao.CorrectionNotebookService.Infrastructure.Repositories;

public class ExamLevelRepository : Repository<ExamLevel, Guid>, IExamLevelRepository
{
    /// <summary>
    /// 是否存在子级
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<bool> ExistsChildrenAsync(Guid id)
    {
        return Queryable.AnyAsync(x => x.ParentId == id);
    }

    /// <summary>
    /// 根据科目Id和用户获取考试级别
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<ExamLevel> GetWithSubjectByUserAsync(Guid id, Guid userId)
    {
        var result = await Queryable.Where(x => x.Id == id).Select(x => new ExamLevel
        {
            Id = x.Id,
            Name = x.Name,
            IsBuiltIn = x.IsBuiltIn,
            CreatorId = x.CreatorId,
            CreateTime = x.CreateTime,
            ParentId = x.ParentId,
            Subjects = x.Subjects.Where(s => s.CreatorId == userId || s.IsBuiltIn).Select(s => new Subject
            {
                Id = s.Id,
                Name = s.Name,
                Icon = s.Icon,
                QuestionCount = s.Questions.Count(q => q.CreatorId == userId),
                IsBuiltIn = s.IsBuiltIn,
                Sorts = s.Sorts.Where(sort => sort.CreatorId == userId || sort.IsBuiltIn).ToList()
            }).ToList()
        }).FirstOrDefaultAsync();

        // 排序
        foreach (var subject in result.Subjects)
        {
            var sort = subject.Sorts.FirstOrDefault(s => s.CreatorId == userId) ?? subject.Sorts.FirstOrDefault(s => s.IsBuiltIn);
            subject.Sort = sort;
        }

        result.Subjects = [.. result.Subjects.OrderBy(x => x.Sort.Priority)];

        return result;
    }

    /// <summary>
    /// 根据用户获取考试级别列表
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IReadOnlyList<ExamLevel>> GetListByUserAsync(ExamLevelQuery query)
    {
        return await Queryable.Where(query.Criteria).OrderBy(query.OrderBy).ToListAsync();
    }
}