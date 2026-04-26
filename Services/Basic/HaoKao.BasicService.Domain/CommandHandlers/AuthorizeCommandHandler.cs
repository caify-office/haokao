using HaoKao.BasicService.Domain.Commands.Authorize;
using HaoKao.BasicService.Domain.Entities;
using HaoKao.BasicService.Domain.Repositories;
using HaoKao.Common.Extensions;

namespace HaoKao.BasicService.Domain.CommandHandlers;

public class AuthorizeCommandHandler(
    IMediatorHandler bus,
    IUnitOfWork<ServicePermission> unitOfWork,
    IServicePermissionRepository servicePermissionRepository,
    IServiceDataRuleRepository serviceDataRuleRepository
) : CommandHandler(unitOfWork, bus),
    IRequestHandler<NeedAuthorizeListCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IServicePermissionRepository _servicePermissionRepository = servicePermissionRepository ?? throw new ArgumentNullException(nameof(servicePermissionRepository));
    private readonly IServiceDataRuleRepository _serviceDataRuleRepository = serviceDataRuleRepository ?? throw new ArgumentNullException(nameof(serviceDataRuleRepository));

    public async Task<bool> Handle(NeedAuthorizeListCommand request, CancellationToken cancellationToken)
    {
        var oldSps = await _servicePermissionRepository.GetWhereAsync(x => request.ServicePermissionCommandModels
                                                                                  .Select(y => x.ServiceId)
                                                                                  .Contains(x.ServiceId));
        await _servicePermissionRepository.DeleteRangeAsync(oldSps);

        var newSps = request.ServicePermissionCommandModels.Select(x => new ServicePermission
        {
            ServiceId = x.ServiceId,
            ServiceName = x.ServiceName,
            Tag = x.Tag,
            Order = x.Order,
            FuncModule = x.FuncModule,
            OtherParams = x.OtherParams,
            OperationPermissions = x.OperationPermissionModels,
            Permissions = x.Permissions,
        }).ToList();

        await _servicePermissionRepository.AddRangeAsync(newSps);

        #region ServiceDataRule

        var entityTypeNames = request.ServiceDataRuleCommandModels.Select(x => x.EntityTypeName);
        var fieldNames = request.ServiceDataRuleCommandModels.Select(x => x.FieldName);
        var fieldTypes = request.ServiceDataRuleCommandModels.Select(x => x.FieldType);

        var oldRules = await _serviceDataRuleRepository
            .GetWhereAsync(x => entityTypeNames.Contains(x.EntityTypeName)
                             && fieldNames.Contains(x.FieldName)
                             && fieldTypes.Contains(x.FieldType));
        var newRules = request.ServiceDataRuleCommandModels.Select(x => new ServiceDataRule
        {
            EntityTypeName = x.EntityTypeName,
            EntityDesc = x.EntityDesc,
            FieldName = x.FieldName,
            FieldType = x.FieldType,
            FieldValue = x.FieldValue,
            FieldDesc = x.FieldDesc,
            ExpressionType = x.ExpressionType,
            UserType = x.UserType,
            Tag = x.Tag,
            Order = x.Order
        }).ToList();
        await _serviceDataRuleRepository.DeleteRangeAsync(oldRules);
        await _serviceDataRuleRepository.AddRangeAsync(newRules);

        #endregion

        if (await Commit())
        {
            await _bus.RemoveListCacheEvent<ServiceDataRule>(cancellationToken);
            await _bus.RemoveListCacheEvent<ServicePermission>(cancellationToken);
        }

        return true;
    }
}