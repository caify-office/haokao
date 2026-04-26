namespace HaoKao.StudentService.Domain.Works;

public interface ISyncStudentFollowWork : IManager
{
    Task ExecuteAsync();
}

public interface IUpdateStudentFollowWork : IManager
{
    Task ExecuteAsync();
}