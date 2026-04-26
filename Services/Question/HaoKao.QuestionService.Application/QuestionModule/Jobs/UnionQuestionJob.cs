using Girvs.Quartz;
using HaoKao.QuestionService.Domain.Works;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace HaoKao.QuestionService.Application.QuestionModule.Jobs;

public class UnionQuestionJob(IServiceProvider serviceProvider) : GirvsJob(serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    public override async void GirvsExecute(IJobExecutionContext context)
    {
        await _serviceProvider.GetRequiredService<IUnionQuestionWork>().ExecuteAsync();
    }
}