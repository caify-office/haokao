namespace HaoKao.StudentService.Domain.Works;

public interface IAutoAllocationWork : IManager
{
    Task ExecuteAsync();
}