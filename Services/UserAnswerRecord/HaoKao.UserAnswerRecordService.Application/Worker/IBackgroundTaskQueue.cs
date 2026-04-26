namespace HaoKao.UserAnswerRecordService.Application.Worker;

public interface IBackgroundTaskQueue
{
    /// <summary>
    /// 入队列
    /// </summary>
    /// <param name="workItem"></param>
    /// <returns></returns>
    ValueTask EnqueueAsync(Func<CancellationToken, ValueTask> workItem);

    /// <summary>
    /// 出队列
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken);
}