using Girvs.Quartz;
using HaoKao.CourseService.Application.Modules.CourseModule.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;

namespace HaoKao.CourseService.Application.Jobs;

public class MergeChapterAndVideoJob(IServiceProvider serviceProvider, ILogger<MergeChapterAndVideoJob> logger) : GirvsJob(serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new NotImplementedException(nameof(serviceProvider));

    public override async void GirvsExecute(IJobExecutionContext context)
    {
        try
        {
            logger.LogInformation("开始执行合并课程章节表和视频表定时任务");
            await _serviceProvider.GetRequiredService<ICourseService>().MergeChaperAndVideo();
            logger.LogInformation("完成执行合并课程章节表和视频表定时任务");
        }
        catch (Exception e)
        {
            logger.LogError(e, "合并课程章节表和视频表定时任务出现异常");
        }
    }
}