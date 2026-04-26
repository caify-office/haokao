using HaoKao.Common.Extensions;
using HaoKao.TenantService.Domain.Entities;
using HaoKao.TenantService.Domain.Enums;

namespace HaoKao.TenantService.Domain.Commands;

public class TenantCommandHandler(
    IUnitOfWork<Tenant> uow,
    IMediatorHandler bus,
    ITenantRepository repository
) : CommandHandler(uow, bus),
    IRequestHandler<CreateTenantCommand, bool>,
    IRequestHandler<UpdateTenantCommand, bool>,
    IRequestHandler<SetPaymentConfigTenantCommand, bool>,
    IRequestHandler<EnableDisabledExamCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly ITenantRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<bool> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        var exist = await _repository.GetByNoAsync(request.TenantNo);

        if (exist != null)
        {
            await _bus.RaiseEvent(new DomainNotification(nameof(request.TenantNo), "代码已存在"), cancellationToken);
            return false;
        }

        var tenant = new Tenant
        {
            OtherId = request.OtherId,
            OtherName = request.OtherName,
            TenantName = request.TenantName,
            TenantNo = request.TenantNo,
            ReleaseState = ReleaseState.NotRelease,
            StartState = EnableState.Enable,
            AdminUserAcount = request.AdminUserAcount,
            AdminUserName = request.AdminUserName,
            AdminPhone = request.AdminPhone,
            SystemModule = request.SystemModule,
            AnnualExamTime = request.AnnualExamTime,
            CreateTime = DateTime.Now
        };

        await _repository.AddAsync(tenant);

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<Tenant>.ByIdCacheKey.Create(tenant.Id.ToString());
            // 添加到缓存Key中
            await _bus.RaiseEvent(new SetCacheEvent(tenant, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<Tenant>(cancellationToken);


            // 发布添加租户管理员事件
            var createExamAdminEvent = new CreateUserDomainEvent(
                request.AdminUserAcount,
                request.AdminPassWord,
                request.AdminUserName,
                request.AdminPhone,
                UserType.GeneralUser,
                tenant.Id,
                request.TenantName
            );
            await _bus.RaiseEvent(createExamAdminEvent, cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
    {
        var exist = await _repository.GetByNoAsync(request.TenantNo);

        if (exist != null && exist.Id != request.Id)
        {
            await _bus.RaiseEvent(new DomainNotification(nameof(request.TenantNo), "代码已存在"), cancellationToken);
            return false;
        }

        var tenant = await _repository.GetByIdAsync(request.Id);
        if (tenant == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应的数据"), cancellationToken);
            return false;
        }

        tenant.OtherId = request.OtherId;
        tenant.OtherName = request.TenantName;
        tenant.TenantName = request.TenantName;
        tenant.TenantNo = request.TenantNo;
        tenant.AdminUserName = request.AdminUserName;
        tenant.AdminPhone = request.AdminPhone;
        tenant.SystemModule = request.SystemModule;
        tenant.AnnualExamTime = request.AnnualExamTime;

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<Tenant>.ByIdCacheKey.Create(tenant.Id.ToString());
            // 添加到缓存Key中
            await _bus.RaiseEvent(new SetCacheEvent(tenant, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<Tenant>(cancellationToken);
        }

        // 发布修改租户管理员事件
        var createExamAdminEvent = new EditUserDomainEvent(
            request.Id,
            request.TenantName,
            tenant.AdminUserAcount,
            request.AdminPassWord,
            request.AdminUserName,
            request.AdminPhone
        );
        await _bus.RaiseEvent(createExamAdminEvent, cancellationToken);

        return true;
    }

    public async Task<bool> Handle(SetPaymentConfigTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = await _repository.GetByIdAsync(request.Id);
        if (tenant == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应的数据"), cancellationToken);
            return false;
        }

        tenant.PaymentConfigs = request.PaymentConfigs;

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<Tenant>.ByIdCacheKey.Create(tenant.Id.ToString());

            // 添加到缓存Key中
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
            await _bus.RaiseEvent(new SetCacheEvent(tenant, key, key.CacheTime), cancellationToken);

           var listKey = new RemoveCacheListEvent(GirvsEntityCacheDefaults<Tenant>.ListCacheKey.Create());
            await _bus.RaiseEvent(listKey, cancellationToken);
        }
        return true;
    }

    public async Task<bool> Handle(EnableDisabledExamCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByIdAsync(request.Id);
        if (result == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应的数据"), cancellationToken);
            return false;
        }

        result.StartState = request.StartState;

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<Tenant>.ByIdCacheKey.Create(result.Id.ToString());
            // 添加到缓存Key中
            await _bus.RaiseEvent(new SetCacheEvent(result, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<Tenant>(cancellationToken);
        }

        return true;
    }
}