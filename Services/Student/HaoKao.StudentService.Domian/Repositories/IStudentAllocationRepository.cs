using HaoKao.StudentService.Domain.Entities;
using HaoKao.StudentService.Domain.Queries;

namespace HaoKao.StudentService.Domain.Repositories;

public interface IStudentAllocationRepository : IRepository<StudentAllocation>
{
    Task<StudentAllocationQuery> GetByStudentAllocationQueryAsync(StudentAllocationQuery query);
}