using HaoKao.StudentService.Domain.Works;

namespace HaoKao.StudentService.Application.Jobs;

public class StudentAllocationJob(
    IServiceProvider provider,
    ILogger<StudentAllocationJob> logger
) : GirvsJob(provider)
{
    private readonly IServiceProvider _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    private readonly ILogger<StudentAllocationJob> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public override async void GirvsExecute(IJobExecutionContext context)
    {
        try
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<ISyncStudentFollowWork>().ExecuteAsync();
            await scope.ServiceProvider.GetRequiredService<IAutoAllocationWork>().ExecuteAsync();
            await scope.ServiceProvider.GetRequiredService<IUpdateStudentFollowWork>().ExecuteAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "学员分配任务执行失败");
        }
    }
}