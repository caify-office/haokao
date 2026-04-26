using Girvs.Quartz;
using HaoKao.UserAnswerRecordService.Domain.Works;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace HaoKao.UserAnswerRecordService.Application.Jobs;

public class UnionAnswerRecordJob(IServiceProvider serviceProvider) : GirvsJob(serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    public override async void GirvsExecute(IJobExecutionContext context)
    {
        await _serviceProvider.GetRequiredService<IUnionAnswerRecordWork>().ExecuteAsync();
    }
}