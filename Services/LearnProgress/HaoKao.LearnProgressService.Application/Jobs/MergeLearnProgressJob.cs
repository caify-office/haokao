using Girvs.Quartz;
using HaoKao.LearnProgressService.Application.AppService;
using HaoKao.LearnProgressService.Application.AppService.ManageMent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;

namespace HaoKao.LearnProgressService.Application.Jobs;

public class MergeLearnProgressJob(IServiceProvider serviceProvider, ILogger<MergeLearnProgressJob> _logger):GirvsJob(serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider??throw new NullReferenceException(nameof(serviceProvider));

    public override async void GirvsExecute(IJobExecutionContext context)
    {
        // 执行0点的任务
        _logger.LogInformation("开始执行合并听课进度表定时任务");

        // 调用执行任务的逻辑
        await serviceProvider.GetRequiredService<ILearnProgressService>().MergeLearnProgress();


        // 执行0点的任务
        _logger.LogInformation("完成执行合并听课进度表定时任务");
    }
}