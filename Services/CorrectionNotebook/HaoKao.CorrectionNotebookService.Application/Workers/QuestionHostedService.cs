using HaoKao.CorrectionNotebookService.Application.Options;
using HaoKao.CorrectionNotebookService.Domain.Commands;
using HaoKao.CorrectionNotebookService.Domain.Entities;
using HaoKao.CorrectionNotebookService.Domain.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace HaoKao.CorrectionNotebookService.Application.Workers;

public interface IQuestionQueue
{
    bool IsCompleted { get; }

    ValueTask EnqueueAsync(Question question, CancellationToken cancellationToken = default);

    ValueTask<Question> DequeueAsync(CancellationToken cancellationToken = default);
}

public class QuestionQueue : IQuestionQueue
{
    private readonly Channel<Question> _channel = Channel.CreateUnbounded<Question>();

    public bool IsCompleted => _channel.Reader.Count == 0;

    public ValueTask EnqueueAsync(Question question, CancellationToken cancellationToken = default)
    {
        return _channel.Writer.WriteAsync(question, cancellationToken);
    }

    public ValueTask<Question> DequeueAsync(CancellationToken cancellationToken = default)
    {
        return _channel.Reader.ReadAsync(cancellationToken);
    }
}

public class QuestionHostedService(
    IQuestionQueue queue,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IServiceFactory<IOcrService> ocrFactory,
    IServiceFactory<ILargeLanguageModel> llmFactory,
    ILogger<QuestionHostedService> logger
) : BackgroundService
{
    private readonly IQuestionQueue _queue = queue ?? throw new ArgumentNullException(nameof(queue));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IServiceFactory<IOcrService> _ocrFactory = ocrFactory ?? throw new ArgumentNullException(nameof(ocrFactory));
    private readonly IServiceFactory<ILargeLanguageModel> _llmFactory = llmFactory ?? throw new ArgumentNullException(nameof(llmFactory));
    private readonly ILogger<QuestionHostedService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly EnabledServiceOptions _options = Singleton<AppSettings>.Instance.Get<EnabledServiceOptions>();

    // 限制并发量 默认10QPS
    private readonly SemaphoreSlim _semaphore = new(10);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Queued Hosted Service is running.{NewLine}", Environment.NewLine);

        while (!stoppingToken.IsCancellationRequested)
        {

            var question = await _queue.DequeueAsync(stoppingToken);

            await _semaphore.WaitAsync(stoppingToken);

#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    // 模拟耗时操作
                    //await Task.Delay(TimeSpan.FromSeconds(4));
                    //Console.WriteLine("Task Complete " + Guid.NewGuid());

                    var ocrId = _options.OcrService.Id;
                    var ocrService = _ocrFactory.Create(ocrId);
                    question.Content = await ocrService.Scan(question.ImageUrl);

                    var llmId = _options.FreeLLMService.Id;
                    var llm = _llmFactory.Create(llmId);
                    var content = _propmt + question.Content;
                    await question.GenerateAnswerAndAnalysis(llm, content);

                    var command = new SaveAnswerAndAnalysisCommand(question);
                    await _bus.TrySendCommand(command, _notifications, stoppingToken);

                    var cacheKey = $"correction_notebook:user_{question.CreatorId}:{typeof(Question).Name.ToLower()}:id:{question.Id}";
                    await _bus.RemoveCache(new CacheKey(cacheKey).Create(), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Dequeue operation was cancelled.{NewLine}", Environment.NewLine);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred executing AddMessage.{NewLine}", Environment.NewLine);
                }
                finally
                {
                    _semaphore.Release();
                }
            }, stoppingToken);
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
        }
    }

    private static readonly string _propmt = """
# 角色任务
作为做题专家，你需要理解题目并给出答案及详细解析。你需要具备广泛的知识储备，能够解答各种类型的题目。
包括数学、物理、化学、生物、历史、地理、语言、编程、建筑、消防、经济、法律、金融、会计、社会工作等方面的题目。

# 工具能力
1. 知识储备
你需要拥有广泛的知识储备，能够解答各种类型的题目。

# 要求与限制
1. 准确性
在给出答案和解析时，必须确保准确无误。
2. 详细解析
除了给出答案，还需要给出详细的解析过程，帮助用户理解题目的解题思路和方法。
3. 限制
应用应限制题目类型和难度范围，确保你能处理大部分常见题目，但对于过于复杂或超出范围的题目，应通过网络搜索来解答。
输出结果的格式要求：答案不超过100字，解析不超过200字。
必须以纯文本的格式输出答案和解析，格式必须为：
答案: {参考答案}
解析: {解析内容}

面对多道题目的时候，你应该把多个答案放在一起，多个解析放在一起，格式：
答案: {参考答案1、参考答案2……}
解析: {解析1、解析2、解析3……}

# 示例
当面对一道关于经济学的选择题时，你会：
答案: 仔细审题，理解题目的关键点，然后结合题目要求和信息，分析出正确答案。
解析: 本题考察的是经济学中的供需关系，通过审题和分析，可以看出供给与需求之间的平衡关系，因此答案是X。

# 题目

""";
}