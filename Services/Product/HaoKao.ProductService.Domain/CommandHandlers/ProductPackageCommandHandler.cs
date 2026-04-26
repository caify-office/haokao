using HaoKao.ProductService.Domain.Commands.ProductPackage;
using HaoKao.ProductService.Domain.Entities;
using HaoKao.ProductService.Domain.Repositories;

namespace HaoKao.ProductService.Domain.CommandHandlers;

public class ProductPackageCommandHandler(
    IUnitOfWork<ProductPackage> uow,
    IProductPackageRepository repository,
    IMediatorHandler bus,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateProductPackageCommand, bool>,
    IRequestHandler<UpdateProductPackageCommand, bool>,
    IRequestHandler<SetProductPackageEnableCommand, bool>,
    IRequestHandler<SetProductPackageProductListCommand, bool>,
    IRequestHandler<SetProductPackageShelvesCommand, bool>,
    IRequestHandler<DeleteProductPackageCommand, bool>
{
    private readonly IProductPackageRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateProductPackageCommand request, CancellationToken cancellationToken)
    {
        var package = mapper.Map<ProductPackage>(request);
        if (package.IsExperience)
        {
            //体验产品开启后“热门推荐”、“课程对比”不能打开，如已打开则自动关闭；
            package.Contrast = false;
            package.Hot = false;
        }

        await _repository.AddAsync(package);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(package, package.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<ProductPackage>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateProductPackageCommand request, CancellationToken cancellationToken)
    {
        var package = await _repository.GetByIdAsync(request.Id);
        if (package == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应产品包的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }
        package = mapper.Map(request, package);

        if (package.IsExperience)
        {
            if (package.ProductList.Count != 0)
            {
                await _bus.RaiseEvent(
                    new DomainNotification("", "该产品包已绑定了产品，请先在“设置产品”中解除绑定后再操作", StatusCodes.Status400BadRequest),
                    cancellationToken);
                return false;
            }
            //体验产品开启后“热门推荐”、“课程对比”不能打开，如已打开则自动关闭；
            package.Contrast = false;
            package.Hot = false;
        }
        await _repository.UpdateAsync(package);
        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(package, package.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<ProductPackage>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(SetProductPackageEnableCommand request, CancellationToken cancellationToken)
    {
        var package = await _repository.GetByIdAsync(request.Id);
        if (package == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应产品包的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        package.Enable = request.Enable;

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(package, package.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<ProductPackage>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(SetProductPackageProductListCommand request, CancellationToken cancellationToken)
    {
        var package = await _repository.GetByIdAsync(request.Id);
        if (package == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应产品包的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        package.ProductList = request.ProductList;

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(package, package.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<ProductPackage>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(SetProductPackageShelvesCommand request, CancellationToken cancellationToken)
    {
        var package = await _repository.GetByIdAsync(request.Id);
        if (package == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应产品包的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }
        if (!package.Enable && request.Shelves)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "请先启用产品包再上架", StatusCodes.Status400BadRequest),
                cancellationToken);
            return false;
        }
        package.Shelves = request.Shelves;

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(package, package.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<ProductPackage>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteProductPackageCommand request, CancellationToken cancellationToken)
    {
        var package = await _repository.GetByIdAsync(request.Id);
        if (package == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应产品包的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(package);

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<ProductPackage>(package.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<ProductPackage>(cancellationToken);
        }

        return true;
    }
}