using HaoKao.Common.Enums;
using HaoKao.ProductService.Domain.Commands.StudentPermission;
using HaoKao.ProductService.Domain.Entities;
using HaoKao.ProductService.Domain.Enums;
using HaoKao.ProductService.Domain.Repositories;
using Newtonsoft.Json;

namespace HaoKao.ProductService.Domain.CommandHandlers;

public class StudentPermissionCommandHandler(
    IUnitOfWork<StudentPermission> uow,
    IMediatorHandler bus,
    IStudentPermissionRepository repository,
    IStudentPermissionOperateLogRepository studentPermissionOperateLogRepository,
    IProductRepository productRepository
) : CommandHandler(uow, bus),
    IRequestHandler<CreateStudentPermissionCommand, bool>,
    IRequestHandler<CreateStudentPermissionEventCommand, bool>,
    IRequestHandler<UpdateStudentPermissionStateCommand, bool>,
    IRequestHandler<UpdateStudentPermissionExpiryTimeCommand, bool>,
    IRequestHandler<DeleteStudentPermissionCommand, bool>,
    IRequestHandler<PurchasingUpdateStudentPermissionCommand, bool>,
    IRequestHandler<UpdateStudentPermissionExpiryTimeEventCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateStudentPermissionCommand request, CancellationToken cancellationToken)
    {
        var products = await productRepository.GetWithGiftsAsync(request.ProductId);
        if (products.Count == 0)
        {
            await _bus.RaiseEvent(new DomainNotification(request.ProductId.ToString(), "未找到对应产品", StatusCodes.Status404NotFound), cancellationToken);
            return false;
        }

        // 查询学员已有的权限
        var permissions = await repository.GetEnabledAndNotExpiredByUserId(request.StudentId);
        var flag = permissions.Any(x => x.ProductId == request.ProductId);
        if (flag)
        {
            await _bus.RaiseEvent(new DomainNotification(request.StudentId.ToString(), "学员已经拥有该权限", StatusCodes.Status400BadRequest), cancellationToken);
            return false;
        }

        // 过滤掉已经有的权限
        var list = products.Where(x => !permissions.Any(y => x.Id == y.ProductId))
                           .Select(x => new StudentPermission
                           {
                               Id = Guid.NewGuid(),
                               StudentName = request.StudentName,
                               StudentId = request.StudentId,
                               OrderNumber = request.OrderNumber,
                               SourceMode = SourceMode.BackgroundAdd,
                               ProductId = x.Id,
                               ProductName = x.Name,
                               ProductType = x.ProductType,
                               ExpiryTime = request.ExpiryTime,
                               Enable = request.Enable
                           }).ToList();

        await repository.AddRangeAsync(list);

        await studentPermissionOperateLogRepository.AddRangeAsync(list.Select(x => new StudentPermissionOperateLog
        {
            StudentPermissionId = x.Id,
            StudentId = x.StudentId,
            StudentName = x.StudentName,
            ProductId = x.ProductId,
            ProductName = x.ProductName,
            ProductType = x.ProductType,
            OldExpiredTime = null,
            NewExpiredTime = x.ExpiryTime
        }).ToList());

        if (await Commit())
        {
           await _bus.RemoveListCacheEvent<StudentPermission>(cancellationToken);
            await _bus.RemoveListCacheEvent<StudentPermissionOperateLog>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(CreateStudentPermissionEventCommand request, CancellationToken cancellationToken)
    {
        //序列化转换
        var contents = JsonConvert.DeserializeObject<List<PurchaseProductContent>>(request.PurchaseProductContents);
        var productIds = contents.Select(x => x.ContentId).ToList();

        // 查询产品
        var products = request.SourceMode == SourceMode.GiftBag
            ? await productRepository.GetWithGiftsAsync(productIds) //活动获取，需要查询对应赠品
            : await productRepository.GetByIdsAsync(productIds);

        // 查询学员已有的权限
        var permissions = await repository.GetEnabledAndNotExpiredByUserId(request.StudentId);
        // 过滤掉已经有的权限
        var list = products.Where(x => !permissions.Any(y => x.Id == y.ProductId))
                           .Select(x => new StudentPermission
                           {
                               StudentName = request.StudentName,
                               StudentId = request.StudentId,
                               OrderNumber = request.OrderNumber,
                               SourceMode = request.SourceMode,
                               ProductId = x.Id,
                               ProductName = x.Name,
                               ProductType = x.ProductType,
                               ExpiryTime = x.ExpiryTimeTypeEnum == ExpiryTimeTypeEnum.Date ? x.ExpiryTime : DateTime.Now.AddDays(x.Days),
                               Enable = request.Enable
                           }).ToList();

        await repository.AddRangeAsync(list);

        if (await Commit())
        {
           await _bus.RemoveListCacheEvent<StudentPermission>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateStudentPermissionStateCommand request, CancellationToken cancellationToken)
    {
        var studentPermission = await repository.GetByIdAsync(request.Id);
        if (studentPermission == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应学员权限表的数据", StatusCodes.Status404NotFound),
                                  cancellationToken);
            return false;
        }

        studentPermission.Enable = request.Enable;

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<StudentPermission>.ByIdCacheKey.Create(studentPermission.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(studentPermission, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<StudentPermission>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteStudentPermissionCommand request, CancellationToken cancellationToken)
    {
        var studentPermission = await repository.GetByIdAsync(request.Id);
        if (studentPermission == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应学员权限表的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await repository.DeleteAsync(studentPermission);

        if (await Commit())
        {
            var key = GirvsEntityCacheDefaults<StudentPermission>.ByIdCacheKey.Create(studentPermission.Id.ToString());
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
           await _bus.RemoveListCacheEvent<StudentPermission>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(PurchasingUpdateStudentPermissionCommand request, CancellationToken cancellationToken)
    {
        var ps = request.Products.Select(product => new StudentPermission
        {
            StudentName = request.StudentName,
            StudentId = request.StudentId,
            OrderNumber = request.OrderNumber,
            ProductId = product.Key,
            ProductName = product.Value,
            ExpiryTime = request.ExpiryTime,
            Enable = request.Enable
        }).ToList();

        await repository.AddRangeAsync(ps);

        if (await Commit())
        {
           await _bus.RemoveListCacheEvent<StudentPermission>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateStudentPermissionExpiryTimeCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应学员权限表的数据", StatusCodes.Status404NotFound),
                                  cancellationToken);
            return false;
        }

        await studentPermissionOperateLogRepository.AddAsync(new StudentPermissionOperateLog
        {
            StudentPermissionId = entity.Id,
            StudentId = entity.StudentId,
            StudentName = entity.StudentName,
            ProductId = entity.ProductId,
            ProductName = entity.ProductName,
            ProductType = entity.ProductType,
            OldExpiredTime = entity.ExpiryTime,
            NewExpiredTime = request.ExpiryTime,
        });

        entity.ExpiryTime = request.ExpiryTime;

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<StudentPermission>.ByIdCacheKey.Create(entity.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(entity, key, key.CacheTime), cancellationToken);
            // 删除查询相关的缓存(事件触发的命令，缓存键OtherQueryConditionKey部分和管理端对不上，需要使用TenantListCacheKey)
            await _bus.RemoveListCacheEvent<StudentPermission>(cancellationToken);
            await _bus.RemoveListCacheEvent<StudentPermissionOperateLog>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateStudentPermissionExpiryTimeEventCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetAsync(x => x.StudentId == request.StudentId && x.ProductId == request.ProductId);
        if (entity == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.StudentId.ToString(), "未找到对应学员权限表的数据", StatusCodes.Status404NotFound),
                                  cancellationToken);
            return false;
        }

        entity.ExpiryTime = request.ExpiryTime;

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<StudentPermission>.ByIdCacheKey.Create(entity.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(entity, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<StudentPermission>(cancellationToken);
        }

        return true;
    }
}