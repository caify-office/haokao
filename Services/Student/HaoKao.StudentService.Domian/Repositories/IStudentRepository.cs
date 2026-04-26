using HaoKao.StudentService.Domain.Entities;
using HaoKao.StudentService.Domain.Queries;

namespace HaoKao.StudentService.Domain.Repositories;

public interface IStudentRepository : IRepository<Student>
{
    Task<Student> GetIncludeAsync(Expression<Func<Student, bool>> predicate);

    Task<StudentQuery> GetByStudentQueryAsync(StudentQuery query);

    Task AutoMigrationAsync();

    Task<RegisterUserPageDto> GetRegisterUsers(
       DateTime start, DateTime end,
       bool? isFollowed, bool? isBandingWeiXin,
       int pageIndex, int pageSize
    );
}

public record RegisterUserPageDto
{
    public int Total { get; init; }
    public IReadOnlyList<RegisterUserDto> Data { get; init; }
}

public record RegisterUserDto
{
    public string NickName { get; init; }
    public string Phone { get; init; }
    public string UnionId { get; init; }
    public DateTime CreateTime { get; init; }
    public string FollowUserId { get; init; }
    public string FollowUserName { get; init; }
}