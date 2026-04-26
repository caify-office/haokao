using Girvs.Extensions;
using Girvs.Infrastructure;
using HaoKao.BurialPointService.Domain.Entities;
using HaoKao.Common.Extensions;

namespace HaoKao.BurialPointService.Domain.Commands;

public class BrowseRecordCommandHandler(
    IUnitOfWork<BrowseRecord> uow,
    IBrowseRecordRepository repository,
    IMediatorHandler bus,
    IBurialPointRepository burialPointRepository,
    IRegisterUserRepository registerUserRepository
) : CommandHandler(uow, bus),
    IRequestHandler<CreateBrowseRecordCommand, bool>
{
    private readonly IBurialPointRepository _burialPointRepository = burialPointRepository ?? throw new ArgumentNullException(nameof(burialPointRepository));
    private readonly IBrowseRecordRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IRegisterUserRepository _registerUserRepository = registerUserRepository ?? throw new ArgumentNullException(nameof(registerUserRepository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateBrowseRecordCommand request, CancellationToken cancellationToken)
    {
        var creatorId = EngineContext.Current.ClaimManager.GetUserId().ToGuid();
        var register = await _registerUserRepository.Get(creatorId);
        //查找是否已存在埋点
        var burialPoint = await _burialPointRepository.GetAsync(x => x.Name == request.BurialPointName
                                                                  && x.BelongingPortType == request.BelongingPortType);

        if (burialPoint == null)
        {
            burialPoint = new BurialPoint
            {
                Name = request.BurialPointName,
                BelongingPortType = request.BelongingPortType
            };
            await _burialPointRepository.AddAsync(burialPoint);
        }

        var browseRecord = new BrowseRecord
        {
            BurialPointId = burialPoint.Id,
            UserName = EngineContext.Current.ClaimManager.GetUserName(),
            Phone = register?.Phone,
            BrowseData = request.BrowseData,
            IsPaidUser = request.IsPaidUser
        };

        await _repository.AddAsync(browseRecord);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(browseRecord, browseRecord.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<BrowseRecord>(cancellationToken);

            await _bus.UpdateIdCacheEvent(burialPoint, burialPoint.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<BurialPoint>(cancellationToken);
        }

        return true;
    }
}