using Girvs.Infrastructure;
using HaoKao.Common.Extensions;
using HaoKao.LearnProgressService.Domain.Commands.LearnProgress;
using HaoKao.LearnProgressService.Domain.Entities;
using HaoKao.LearnProgressService.Domain.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HaoKao.LearnProgressService.Domain.CommandHandlers;

public class LearnProgressCommandHandler(
    IUnitOfWork<LearnProgress> uow,
    ILearnProgressRepository repository,
    IDailyStudyDurationRepository dailyStudyDurationRepository,
    IMediatorHandler bus
) : CommandHandler(uow, bus),
    IRequestHandler<CreateLearnProgressCommand, bool>,
    IRequestHandler<UpdateLearnProgressCommand, bool>,
    IRequestHandler<DeleteLearnProgressCommand, bool>,
    IRequestHandler<UpdateThVideoProgressCommand, bool>,
    IRequestHandler<UpdateIsEndCommand, bool>
{
    private readonly ILearnProgressRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateLearnProgressCommand request, CancellationToken cancellationToken)
    {
        var learnProgress = await _repository.GetLearnProgressByIdentifier(request.Identifier);
        if (learnProgress == null)
        {
            learnProgress = new LearnProgress
            {
                ProductId = request.ProductId,
                SubjectId = request.SubjectId,
                ChapterId = request.ChapterId,
                CourseId = request.CourseId,
                VideoId = request.VideoId,
                Identifier = request.Identifier,
                Progress = request.Progress,
                TotalProgress = request.TotalProgress,
                MaxProgress = request.MaxProgress
            };
            await _repository.AddAsync(learnProgress);
        }
        else
        {
            //标识符存在,更新最新学习进度，这里的进度和学习时长的更新逻辑
            //对比读取最大的学习进度
            var maxProgress = Math.Max(learnProgress.MaxProgress, request.MaxProgress);
            //计算本次间隔时间（单位小时）
            // 将 dynamic 类型显式转换为 decimal
            var max = Convert.ToDecimal(maxProgress);
            var progress = Convert.ToDecimal(learnProgress.Progress);

            // 所有运算数均为 decimal 类型
            var studyDuration = Math.Round((max - progress) / 3600m, 2, MidpointRounding.AwayFromZero);
            learnProgress.Progress = maxProgress;
            learnProgress.MaxProgress = maxProgress;
            //更新这个时间
            learnProgress.CreateTime = DateTime.Now;
            await DailyStudyDurationsRecord(learnProgress.ProductId, learnProgress.SubjectId, learnProgress.CreatorId, studyDuration);
            await _repository.UpdateAsync(learnProgress);
        }

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(learnProgress, learnProgress.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<LearnProgress>(cancellationToken);
        }

        return true;
    }

    /// <summary>
    /// 更新视频总时长
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> Handle(UpdateThVideoProgressCommand request, CancellationToken cancellationToken)
    {
        //读取当前视频下所有的最新学习进度数据
        var progressResult = _repository.GetWhereAsync(x => x.VideoId == request.VideoId).Result.ToList();
        foreach (var model in progressResult)
        {
            //转换maxProgress
            var progressRate = double.Parse(model.MaxProgress.ToString()) / double.Parse(model.TotalProgress.ToString());
            var factProgress = Math.Round(request.Duration * progressRate);
            //更新实际视频进度
            await _repository.UpdateFactProgress(model.Id, int.Parse(factProgress.ToString()), int.Parse(request.Duration.ToString("f0")));
        }

        return true;
    }

    /// <summary>
    /// 更新视频是否已学完
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> Handle(UpdateIsEndCommand request, CancellationToken cancellationToken)
    {
        var userId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();
        //读取当前视频最新的记录
        var learnProgress = await _repository.GetLearnProgress(request.Id, userId);
        if (learnProgress == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }
        learnProgress.IsEnd = true;
        learnProgress.CreateTime = DateTime.Now;
        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(learnProgress, learnProgress.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<LearnProgress>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateLearnProgressCommand request, CancellationToken cancellationToken)
    {
        var learnProgress = await _repository.GetByIdAsync(request.Id);
        if (learnProgress == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }
        learnProgress.Progress = request.Progress;
        learnProgress.MaxProgress = request.MaxProgress;

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(learnProgress, learnProgress.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<LearnProgress>(cancellationToken);
        }

        return true;
    }

    private async Task DailyStudyDurationsRecord(Guid productId, Guid subjectId, Guid userId, decimal studyDuration)
    {
        //计算学习时长
        var learnTime = DateOnly.FromDateTime(DateTime.Now);
        var dailyStudy = await dailyStudyDurationRepository.GetAsync(x => x.ProductId == productId
                                                                       && x.SubjectId == subjectId
                                                                       && x.CreatorId == userId
                                                                       && x.LearnTime == learnTime);
        if (dailyStudy == null)
        {
            //不存在，则添加
            dailyStudy = new DailyStudyDuration
            {
                ProductId = productId,
                SubjectId = subjectId,
                LearnTime = learnTime,
                DailyVideoStudyDuration = studyDuration
            };
            await dailyStudyDurationRepository.AddAsync(dailyStudy);
        }
        else
        {
            //存在则累加
            dailyStudy.DailyVideoStudyDuration += studyDuration;
            await dailyStudyDurationRepository.UpdateAsync(dailyStudy);
        }
    }

    public async Task<bool> Handle(DeleteLearnProgressCommand request, CancellationToken cancellationToken)
    {
        var learnProgress = await _repository.GetByIdAsync(request.Id);
        if (learnProgress == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(learnProgress);

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<LearnProgress>(learnProgress.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<LearnProgress>(cancellationToken);
        }

        return true;
    }
}