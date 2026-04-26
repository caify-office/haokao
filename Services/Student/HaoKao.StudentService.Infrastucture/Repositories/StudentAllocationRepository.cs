using HaoKao.StudentService.Domain.Entities;
using HaoKao.StudentService.Domain.Queries;
using HaoKao.StudentService.Domain.Repositories;

namespace HaoKao.StudentService.Infrastructure.Repositories;

public class StudentAllocationRepository : Repository<StudentAllocation>, IStudentAllocationRepository
{
    public async Task<StudentAllocationQuery> GetByStudentAllocationQueryAsync(StudentAllocationQuery query)
    {
        query.RecordCount = await Queryable.AsNoTracking().CountAsync(query.GetQueryWhere());
        if (query.RecordCount == 0)
        {
            query.Result = [];
        }
        else
        {
            query.Result = await Queryable.AsNoTracking()
                                          .Include(x => x.Student)
                                          .ThenInclude(x => x.RegisterUser)
                                          .Include(x => x.Student)
                                          .ThenInclude(x => x.StudentFollows)
                                          .Where(query.GetQueryWhere())
                                          .OrderByDescending(x => x.AllocationTime)
                                          .Skip(query.PageStart)
                                          .Take(query.PageSize)
                                          .ToListAsync();
        }
        return query;
    }
}