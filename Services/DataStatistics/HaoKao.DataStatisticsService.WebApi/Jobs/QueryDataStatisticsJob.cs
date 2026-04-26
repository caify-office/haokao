using Girvs.Cache.Caching;
using Girvs.Quartz;
using HaoKao.DataStatisticsService.WebApi.CacheKeyManager;
using HaoKao.DataStatisticsService.WebApi.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace HaoKao.DataStatisticsService.WebApi.Jobs;

public class QueryDataStatisticsJob(
    IServiceProvider serviceProvider,
    ILogger<QueryDataStatisticsJob> logger
) : GirvsJob(serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    public override async void GirvsExecute(IJobExecutionContext context)
    {
        logger.LogInformation("开始设置统计缓存");
        //查询放入缓存
        var tenantScopeSp = _serviceProvider.CreateAsyncScope();
        var service = tenantScopeSp.ServiceProvider.GetRequiredService<ProgressStatisticsService>();
        await service.StatisticsToCache();
        logger.LogInformation("设置统计缓存完成");
    }


   
}


   
