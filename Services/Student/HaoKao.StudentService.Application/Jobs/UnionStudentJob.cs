using HaoKao.StudentService.Domain.Works;

namespace HaoKao.StudentService.Application.Jobs;

public class UnionStudentJob(
    IServiceProvider provider,
    ILogger<UnionStudentJob> logger
) : GirvsJob(provider)
{
    private readonly IServiceProvider _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    private readonly ILogger<UnionStudentJob> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public override async void GirvsExecute(IJobExecutionContext context)
    {
        try
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<IUnionStudentWork>().ExecuteAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UnionStudentJob 执行失败");
        }
    }
}