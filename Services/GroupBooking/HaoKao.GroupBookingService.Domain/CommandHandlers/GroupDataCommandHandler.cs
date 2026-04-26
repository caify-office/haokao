using AutoMapper;
using HaoKao.Common.Extensions;
using HaoKao.GroupBookingService.Domain.Commands.GroupData;
using HaoKao.GroupBookingService.Domain.Entities;
using HaoKao.GroupBookingService.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HaoKao.GroupBookingService.Domain.CommandHandlers;

public class GroupDataCommandHandler(
    IUnitOfWork<GroupData> uow,
    IGroupDataRepository repository,
    IMediatorHandler bus,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateGroupDataCommand, bool>,
    IRequestHandler<UpdateGroupDataCommand, bool>,
    IRequestHandler<DeleteGroupDataCommand, bool>
{
    private readonly IGroupDataRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateGroupDataCommand request, CancellationToken cancellationToken)
    {
        var groupData = mapper.Map<GroupData>(request);

        await _repository.AddAsync(groupData);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(groupData, groupData.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<GroupData>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateGroupDataCommand request, CancellationToken cancellationToken)
    {
        var groupData = await _repository.GetByIdAsync(request.Id);
        if (groupData == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        groupData = mapper.Map(request, groupData);
        await _repository.UpdateAsync(groupData);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(groupData, groupData.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<GroupData>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteGroupDataCommand request, CancellationToken cancellationToken)
    {
        var groupData = await _repository.GetByIdAsync(request.Id);
        if (groupData == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(groupData);

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<GroupData>(groupData.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<GroupData>(cancellationToken);
        }

        return true;
    }
}