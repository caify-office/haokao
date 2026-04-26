namespace HaoKao.StudentService.Domain.Works;

public interface IUnionStudentWork : IManager
{
    Task ExecuteAsync();
}