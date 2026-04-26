using Girvs.Extensions;
using Girvs.Infrastructure;
using HaoKao.Common.Extensions;
using HaoKao.LearnProgressService.Domain.Commands.DailyStudyDuration;
using HaoKao.LearnProgressService.Domain.Entities;
using HaoKao.LearnProgressService.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HaoKao.LearnProgressService.Domain.CommandHandlers;

public class DailyStudyDurationCommandHandler(
    IUnitOfWork<DailyStudyDuration> uow,
    IDailyStudyDurationRepository repository,
    IMediatorHandler bus
) : CommandHandler(uow, bus),
    IRequestHandler<CreateDailyStudyDurationCommand, bool>
{
    private readonly IDailyStudyDurationRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateDailyStudyDurationCommand request, CancellationToken cancellationToken)
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().ToGuid();
        var learnTime = DateOnly.FromDateTime(DateTime.Now);
        var dailyStudyDuration = await _repository.GetAsync(x => x.ProductId == request.ProductId
                                                              && x.SubjectId == request.SubjectId
                                                              && x.CreatorId == userId
                                                              && x.LearnTime == learnTime);
        if (dailyStudyDuration is null)
        {
            dailyStudyDuration = new DailyStudyDuration
            {
                ProductId = request.ProductId,
                SubjectId = request.SubjectId,
                DailyVideoStudyDuration = request.StudyDuration,
                LearnTime = learnTime
            };
            await _repository.AddAsync(dailyStudyDuration);
        }
        else
        {
            dailyStudyDuration.DailyVideoStudyDuration += request.StudyDuration;
            await _repository.UpdateAsync(dailyStudyDuration);
        }

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(dailyStudyDuration, dailyStudyDuration.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<DailyStudyDuration>(cancellationToken);
        }

        return true;
    }
}