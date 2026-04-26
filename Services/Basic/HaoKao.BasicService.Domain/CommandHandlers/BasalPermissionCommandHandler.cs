using HaoKao.BasicService.Domain.Commands.BasalPermission;
using HaoKao.BasicService.Domain.Entities;
using HaoKao.BasicService.Domain.Events;
using HaoKao.BasicService.Domain.Repositories;

namespace HaoKao.BasicService.Domain.CommandHandlers;

public class BasalPermissionCommandHandler(
    IMediatorHandler bus,
    IPermissionRepository permissionRepository,
    IUnitOfWork<BasalPermission> unitOfWork
) : CommandHandler(unitOfWork, bus),
    IRequestHandler<SavePermissionCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IPermissionRepository _permissionRepository = permissionRepository ?? throw new ArgumentNullException(nameof(permissionRepository));

    public async Task<bool> Handle(SavePermissionCommand request, CancellationToken cancellationToken)
    {
        var oldPermissions = await _permissionRepository.GetWhereAsync(x => x.AppliedObjectType == request.AppliedObjectType
                                                                         && request.ValidateObjectType == x.ValidateObjectType
                                                                         && request.AppliedId == x.AppliedId);

        await _permissionRepository.DeleteRangeAsync(oldPermissions);

        var newPermissions = request.ObjectPermissions.Select(x =>
        {
            var bp = new BasalPermission
            {
                AppliedId = request.AppliedId,
                AppliedObjectId = x.AppliedObjectId,
                AppliedObjectType = request.AppliedObjectType,
                ValidateObjectType = request.ValidateObjectType
            };

            foreach (var permission in x.PermissionOperation)
            {
                bp.SetBit(permission, AccessControlEntry.Allow);
            }

            return bp;
        }).ToList();

        await _permissionRepository.AddRangeAsync(newPermissions);

        if (await Commit())
        {
            await _bus.RaiseEvent(new RemoveServiceCacheEvent(), cancellationToken);
        }

        return true;
    }
}